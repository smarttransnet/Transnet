#pragma warning disable CA1304, CA1311, CA1862
using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Trips;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.TripCategories.UpdateTripCategory;

public sealed record UpdateTripCategoryCommand(
    Guid CategoryId,
    string? CategoryName,
    List<Guid>? UomIds,
    List<NewUomDto>? NewUoms
) : ICommand;

internal sealed class UpdateTripCategoryCommandHandler(
    IApplicationDbContext dbContext,
    IDateTimeProvider dateTimeProvider,
    IUserContext userContext
) : ICommandHandler<UpdateTripCategoryCommand>
{
    public async Task<Result> Handle(
        UpdateTripCategoryCommand request,
        CancellationToken cancellationToken
    ) {
        // 1. Resolve TripCategory
        var category = await dbContext.TripCategories
            .Include(c => c.CategoryMaterials)
            .FirstOrDefaultAsync(c => c.Id == request.CategoryId, cancellationToken);

        if (category == null)
        {
            return Result.Failure(Error.NotFound(
                "TripCategory.NotFound",
                $"Trip category with ID '{request.CategoryId}' was not found."
            ));
        }

        var now = dateTimeProvider.UtcNow;
        var userId = userContext.UserId;

        // Update category name if provided
        if (!string.IsNullOrWhiteSpace(request.CategoryName))
        {
            var newName = request.CategoryName.Trim();
            // Ensure no other active category has this name
            var duplicateName = await dbContext.TripCategories
                .AnyAsync(c => c.Id != category.Id && c.CategoryName.ToLower() == newName.ToLower() && c.IsActive, cancellationToken);
            
            if (duplicateName)
            {
                return Result.Failure(Error.Conflict(
                    "TripCategory.DuplicateName",
                    $"Another active trip category with name '{newName}' already exists."
                ));
            }

            category.CategoryName = newName;
            category.ModifiedDate = now;
            category.ModifiedBy = userId;
        }

        // 2. Resolve UOMs
        var targetUomIds = request.UomIds != null ? new List<Guid>(request.UomIds) : new List<Guid>();

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
                
                Guid resolvedNewUomId;
                if (existingUom == null)
                {
                    var newUom = new Uom
                    {
                        Id = Guid.NewGuid(),
                        UOMCode = uomCode,
                        Description = newUomDto.Description?.Trim(),
                        IsActive = true
                    };
                    dbContext.Uoms.Add(newUom);
                    resolvedNewUomId = newUom.Id;
                }
                else
                {
                    resolvedNewUomId = existingUom.Id;
                    if (!existingUom.IsActive)
                    {
                        existingUom.IsActive = true;
                    }
                }

                if (!targetUomIds.Contains(resolvedNewUomId))
                {
                    targetUomIds.Add(resolvedNewUomId);
                }
            }
        }

        if (targetUomIds.Count == 0)
        {
            return Result.Failure(Error.Failure(
                "TripCategory.UomRequired",
                "At least one UOM is required."
            ));
        }

        // Validate all existing UOMs (check database or Local context if newly added)
        var allUomsFound = true;
        foreach (var uomId in targetUomIds)
        {
            var exists = await dbContext.Uoms.AnyAsync(u => u.Id == uomId, cancellationToken) ||
                         dbContext.Uoms.Local.Any(u => u.Id == uomId);
            if (!exists)
            {
                allUomsFound = false;
                break;
            }
        }

        if (!allUomsFound)
        {
            return Result.Failure(Error.NotFound(
                "UOM.NotFound",
                "One or more provided UOM IDs do not exist."
            ));
        }

        // 3. Reconcile CategoryMaterials
        var existingMappings = category.CategoryMaterials.ToList();

        // Deactivate mappings that are no longer in target list
        foreach (var mapping in existingMappings)
        {
            if (!targetUomIds.Contains(mapping.UOMId) && mapping.IsActive)
            {
                mapping.IsActive = false;
                mapping.ModifiedDate = now;
                mapping.ModifiedBy = userId;
            }
        }

        // Add or reactivate mappings that are in target list
        foreach (var targetId in targetUomIds)
        {
            var existingMapping = existingMappings.FirstOrDefault(m => m.UOMId == targetId);
            if (existingMapping == null)
            {
                // Create new
                var newMapping = new TripCategoryMaterial
                {
                    Id = Guid.NewGuid(),
                    TripCategoryId = category.Id,
                    UOMId = targetId,
                    IsActive = true,
                    CreatedDate = now,
                    CreatedBy = userId
                };
                dbContext.TripCategoryMaterials.Add(newMapping);
            }
            else if (!existingMapping.IsActive)
            {
                // Reactivate
                existingMapping.IsActive = true;
                existingMapping.ModifiedDate = now;
                existingMapping.ModifiedBy = userId;
            }
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
