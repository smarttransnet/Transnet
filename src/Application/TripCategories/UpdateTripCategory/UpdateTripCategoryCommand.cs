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
    Guid Id,
    Guid? CategoryId,
    string? CategoryName,
    Guid? MaterialId,
    string? MaterialName,
    Guid? UomId,
    string? NewUomCode,
    string? NewUomDescription
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
        var mapping = await dbContext.TripCategoryMaterials
            .FirstOrDefaultAsync(cm => cm.Id == request.Id, cancellationToken);

        if (mapping == null)
        {
            return Result.Failure(Error.NotFound(
                "TripCategoryMaterial.NotFound",
                $"Trip category material mapping with ID '{request.Id}' was not found."
            ));
        }

        var now = dateTimeProvider.UtcNow;
        var userId = userContext.UserId;

        // 1. Resolve TripCategory
        TripCategory? category = null;
        if (request.CategoryId.HasValue)
        {
            category = await dbContext.TripCategories
                .FirstOrDefaultAsync(c => c.Id == request.CategoryId.Value, cancellationToken);
            
            if (category == null)
            {
                return Result.Failure(Error.NotFound(
                    "TripCategory.NotFound",
                    $"Trip category with ID '{request.CategoryId}' was not found."
                ));
            }
        }
        else if (!string.IsNullOrWhiteSpace(request.CategoryName))
        {
            var name = request.CategoryName.Trim();
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
            return Result.Failure(Error.Failure(
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
                return Result.Failure(Error.NotFound(
                    "Material.NotFound",
                    $"Material with ID '{request.MaterialId}' was not found."
                ));
            }

            if (material.TripCategoryId != category.Id)
            {
                return Result.Failure(Error.Failure(
                    "Material.CategoryMismatch",
                    "The selected material does not belong to the selected trip category."
                ));
            }
        }
        else if (!string.IsNullOrWhiteSpace(request.MaterialName))
        {
            var name = request.MaterialName.Trim();
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
            return Result.Failure(Error.Failure(
                "Material.Required",
                "Material ID or Name is required."
            ));
        }

        // 3. Resolve UOM
        Guid resolvedUomId;
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
                resolvedUomId = newUom.Id;
            }
            else
            {
                resolvedUomId = existingUom.Id;
                if (!existingUom.IsActive)
                {
                    existingUom.IsActive = true;
                }
            }
        }
        else
        {
            if (!request.UomId.HasValue)
            {
                return Result.Failure(Error.Failure(
                    "TripCategoryMaterial.UomRequired",
                    "UOM ID is required when not creating a new UOM."
                ));
            }
            resolvedUomId = request.UomId.Value;

            // Verify UOM exists
            var uomExists = await dbContext.Uoms.AnyAsync(u => u.Id == resolvedUomId, cancellationToken);
            if (!uomExists)
            {
                return Result.Failure(Error.NotFound(
                    "UOM.NotFound",
                    $"UOM with ID '{resolvedUomId}' was not found."
                ));
            }
        }

        // 4. Prevent duplicate combinations (excluding current mapping ID)
        var duplicateExists = await dbContext.TripCategoryMaterials
            .AnyAsync(cm => 
                cm.Id != request.Id && 
                cm.TripCategoryId == category.Id && 
                cm.MaterialId == material.Id && 
                cm.UOMId == resolvedUomId &&
                cm.IsActive, 
                cancellationToken
            );

        if (duplicateExists)
        {
            return Result.Failure(Error.Conflict(
                "TripCategoryMaterial.Duplicate",
                $"Another active mapping for Category '{category.CategoryName}', Material '{material.MaterialName}', and this UOM already exists."
            ));
        }

        // 5. Apply changes
        mapping.TripCategoryId = category.Id;
        mapping.MaterialId = material.Id;
        mapping.UOMId = resolvedUomId;
        mapping.ModifiedDate = now;
        mapping.ModifiedBy = userId;

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
