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
    Guid? MaterialId,
    string? MaterialName,
    List<Guid>? UomIds,
    string? NewUomCode,
    string? NewUomDescription
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

        // 0. Resolve/create New Uom if provided
        Guid? newCreatedUomId = null;
        if (!string.IsNullOrWhiteSpace(request.NewUomCode))
        {
            var uomCode = request.NewUomCode.Trim().ToUpper();
            var existingUom = await dbContext.Uoms
                .FirstOrDefaultAsync(u => u.UOMCode.ToUpper() == uomCode, cancellationToken);
            
            if (existingUom == null)
            {
                var newUom = new Uom
                {
                    Id = Guid.NewGuid(),
                    UOMCode = uomCode,
                    Description = request.NewUomDescription?.Trim(),
                    IsActive = true
                };
                dbContext.Uoms.Add(newUom);
                newCreatedUomId = newUom.Id;
            }
            else
            {
                newCreatedUomId = existingUom.Id;
                if (!existingUom.IsActive)
                {
                    existingUom.IsActive = true;
                }
            }
        }

        var uomIdsToMap = request.UomIds != null ? new List<Guid>(request.UomIds) : new List<Guid>();
        if (newCreatedUomId.HasValue && !uomIdsToMap.Contains(newCreatedUomId.Value))
        {
            uomIdsToMap.Add(newCreatedUomId.Value);
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

        // 2. Resolve Material
        Material? material = null;
        if (request.MaterialId.HasValue)
        {
            material = await dbContext.Materials
                .FirstOrDefaultAsync(m => m.Id == request.MaterialId.Value, cancellationToken);
            
            if (material == null)
            {
                return Result.Failure<List<Guid>>(Error.NotFound(
                    "Material.NotFound",
                    $"Material with ID '{request.MaterialId}' was not found."
                ));
            }

            // Ensure the material belongs to this category
            if (material.TripCategoryId != category.Id)
            {
                return Result.Failure<List<Guid>>(Error.Failure(
                    "Material.CategoryMismatch",
                    "The selected material does not belong to the selected trip category."
                ));
            }
        }
        else if (!string.IsNullOrWhiteSpace(request.MaterialName))
        {
            var name = request.MaterialName.Trim();
            // Check if material already exists under this category case-insensitively
            material = await dbContext.Materials
                .FirstOrDefaultAsync(m => m.TripCategoryId == category.Id && m.MaterialName.ToLower() == name.ToLower(), cancellationToken);
            
            if (material == null)
            {
                material = new Material
                {
                    Id = Guid.NewGuid(),
                    TripCategoryId = category.Id,
                    MaterialName = name,
                    IsActive = true,
                    CreatedDate = now,
                    CreatedBy = userId
                };
                dbContext.Materials.Add(material);
            }
            else if (!material.IsActive)
            {
                material.IsActive = true;
                material.ModifiedDate = now;
                material.ModifiedBy = userId;
            }
        }

        if (material == null)
        {
            return Result.Failure<List<Guid>>(Error.Failure(
                "Material.Required",
                "Material ID or Name is required."
            ));
        }

        // 3. Resolve mappings for each UOM
        var createdIds = new List<Guid>();
        foreach (var uomId in uomIdsToMap)
        {
            // Verify UOM exists
            var uomExists = await dbContext.Uoms.AnyAsync(u => u.Id == uomId, cancellationToken);
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
                    cm.MaterialId == material.Id && 
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
                        $"The mapping for Category '{category.CategoryName}', Material '{material.MaterialName}', and selected UOM already exists."
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
                    MaterialId = material.Id,
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
