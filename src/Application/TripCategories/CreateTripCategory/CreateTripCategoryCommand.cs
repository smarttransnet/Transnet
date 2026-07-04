#pragma warning disable CA1304, CA1311, CA1862
using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Trips;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.TripCategories.CreateTripCategory;

public sealed record CreateTripCategoryCommand(
    Guid? CategoryId,
    string? CategoryName,
    List<Guid>? UomIds,
    List<NewUomDto>? NewUoms
) : ICommand<List<Guid>>;

internal sealed class CreateTripCategoryCommandHandler(
    IApplicationDbContext dbContext,
    IDateTimeProvider dateTimeProvider,
    IUserContext userContext
) : ICommandHandler<CreateTripCategoryCommand, List<Guid>>
{
    public async Task<Result<List<Guid>>> Handle(
        CreateTripCategoryCommand request,
        CancellationToken cancellationToken
    ) {
        var now = dateTimeProvider.UtcNow;
        var userId = userContext.UserId;

        // 0. Resolve/create New Uoms if provided
        var uomIdsToMap = request.UomIds != null ? new List<Guid>(request.UomIds) : new List<Guid>();

        if (request.NewUoms != null && request.NewUoms.Count > 0)
        {
            foreach (var newUomDto in request.NewUoms)
            {
                if (string.IsNullOrWhiteSpace(newUomDto.Code))
                {
                    continue;
                }
                
                var uomCode = newUomDto.Code.Trim().ToUpper();
                var existingUom = await dbContext.Uoms
                    .FirstOrDefaultAsync(u => u.UOMCode.ToUpper() == uomCode, cancellationToken);
                
                Guid resolvedId;
                if (existingUom == null)
                {
                    var newUomEntity = new Uom
                    {
                        Id = Guid.NewGuid(),
                        UOMCode = uomCode,
                        Description = newUomDto.Description?.Trim(),
                        IsActive = true
                    };
                    dbContext.Uoms.Add(newUomEntity);
                    resolvedId = newUomEntity.Id;
                }
                else
                {
                    resolvedId = existingUom.Id;
                    if (!existingUom.IsActive)
                    {
                        existingUom.IsActive = true;
                    }
                }

                if (!uomIdsToMap.Contains(resolvedId))
                {
                    uomIdsToMap.Add(resolvedId);
                }
            }
        }

        if (!uomIdsToMap.Any())
        {
            return Result.Failure<List<Guid>>(Error.Failure(
                "TripCategoryMaterial.UomRequired",
                "At least one UOM is required."
            ));
        }

        // 1. Resolve TripCategory
        TripCategory? category = null;
        if (request.CategoryId.HasValue)
        {
            category = await dbContext.TripCategories
                .FirstOrDefaultAsync(c => c.Id == request.CategoryId.Value, cancellationToken);
            
            if (category == null)
            {
                return Result.Failure<List<Guid>>(Error.NotFound(
                    "TripCategory.NotFound",
                    $"Trip category with ID '{request.CategoryId}' was not found."
                ));
            }
        }
        else if (!string.IsNullOrWhiteSpace(request.CategoryName))
        {
            var name = request.CategoryName.Trim();
            // Check if name already exists case-insensitively
            category = await dbContext.TripCategories
                .FirstOrDefaultAsync(c => c.CategoryName.ToLower() == name.ToLower(), cancellationToken);
            
            if (category == null)
            {
                category = new TripCategory
                {
                    Id = Guid.NewGuid(),
                    CategoryName = name,
                    IsActive = true,
                    CreatedDate = now,
                    CreatedBy = userId
                };
                dbContext.TripCategories.Add(category);
            }
            else if (!category.IsActive)
            {
                category.IsActive = true;
                category.ModifiedDate = now;
                category.ModifiedBy = userId;
            }
        }

        if (category == null)
        {
            return Result.Failure<List<Guid>>(Error.Failure(
                "TripCategory.Required",
                "Trip Category ID or Name is required."
            ));
        }

        // 2. Resolve mappings for each UOM
        var createdIds = new List<Guid>();
        foreach (var uomId in uomIdsToMap)
        {
            // Verify UOM exists (or was just added locally in the DbContext)
            var uomExists = await dbContext.Uoms.AnyAsync(u => u.Id == uomId, cancellationToken) ||
                            dbContext.Uoms.Local.Any(u => u.Id == uomId);
            if (!uomExists)
            {
                return Result.Failure<List<Guid>>(Error.NotFound(
                    "UOM.NotFound",
                    $"UOM with ID '{uomId}' was not found."
                ));
            }

            // Check if mapping already exists
            var existingMapping = await dbContext.TripCategoryMaterials
                .FirstOrDefaultAsync(cm => 
                    cm.TripCategoryId == category.Id && 
                    cm.UOMId == uomId, 
                    cancellationToken
                );

            if (existingMapping != null)
            {
                if (existingMapping.IsActive)
                {
                    // Return failure for duplicate combinations
                    return Result.Failure<List<Guid>>(Error.Conflict(
                        "TripCategoryMaterial.Duplicate",
                        $"The mapping for Category '{category.CategoryName}' and selected UOM already exists."
                    ));
                }
                else
                {
                    // Reactivate soft-deleted mapping
                    existingMapping.IsActive = true;
                    existingMapping.ModifiedDate = now;
                    existingMapping.ModifiedBy = userId;
                    createdIds.Add(existingMapping.Id);
                }
            }
            else
            {
                // Create new mapping
                var newMapping = new TripCategoryMaterial
                {
                    Id = Guid.NewGuid(),
                    TripCategoryId = category.Id,
                    UOMId = uomId,
                    IsActive = true,
                    CreatedDate = now,
                    CreatedBy = userId
                };
                dbContext.TripCategoryMaterials.Add(newMapping);
                createdIds.Add(newMapping.Id);
            }
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        return createdIds;
    }
}
