#pragma warning disable CA1304, CA1311, CA1862, IDE0045
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

namespace Application.VehicleCategoryMappings.UpdateMapping;

public sealed record UpdateVehicleCategoryMappingCommand(
    Guid VehicleCategoryId,
    List<Guid>? UomIds,
    List<NewUomDto>? NewUoms
) : ICommand;

internal sealed class UpdateVehicleCategoryMappingCommandHandler(
    IApplicationDbContext dbContext,
    IDateTimeProvider dateTimeProvider,
    IUserContext userContext
) : ICommandHandler<UpdateVehicleCategoryMappingCommand>
{
    public async Task<Result> Handle(
        UpdateVehicleCategoryMappingCommand request,
        CancellationToken cancellationToken
    ) {
        var now = dateTimeProvider.UtcNow;
        var userId = userContext.UserId;

        var category = await dbContext.VehicleCategories
            .FirstOrDefaultAsync(c => c.Id == request.VehicleCategoryId, cancellationToken);
        
        if (category == null)
        {
            return Result.Failure(Error.NotFound(
                "VehicleCategory.NotFound",
                $"Vehicle category with ID '{request.VehicleCategoryId}' was not found."
            ));
        }

        // Resolve/create New Uoms
        var uomIdsToMap = request.UomIds != null ? new List<Guid>(request.UomIds) : new List<Guid>();

        if (request.NewUoms != null && request.NewUoms.Count > 0)
        {
            foreach (var newUomDto in request.NewUoms)
            {
                if (string.IsNullOrWhiteSpace(newUomDto.Code))
                {
                    continue;
                }
                
                var uomCode = newUomDto.Code.Trim().ToUpperInvariant();
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
            return Result.Failure(Error.Failure(
                "VehicleCategoryUom.UomRequired",
                "At least one UOM is required."
            ));
        }

        // Verify UOMs
        foreach (var uomId in uomIdsToMap)
        {
            var uomExists = await dbContext.Uoms.AnyAsync(u => u.Id == uomId, cancellationToken) ||
                            dbContext.Uoms.Local.Any(u => u.Id == uomId);
            if (!uomExists)
            {
                return Result.Failure(Error.NotFound(
                    "UOM.NotFound",
                    $"UOM with ID '{uomId}' was not found."
                ));
            }
        }

        // Fetch existing mappings
        var existingMappings = await dbContext.VehicleCategoryUoms
            .Where(m => m.VehicleCategoryId == category.Id)
            .ToListAsync(cancellationToken);

        // Soft delete mappings no longer in the list
        var toDeactivate = existingMappings
            .Where(m => m.IsActive && !uomIdsToMap.Contains(m.UOMId))
            .ToList();
            
        foreach (var mapping in toDeactivate)
        {
            mapping.IsActive = false;
            mapping.ModifiedDate = now;
            mapping.ModifiedBy = userId;
        }

        // Add or reactivate mappings
        foreach (var uomId in uomIdsToMap)
        {
            var mapping = existingMappings.FirstOrDefault(m => m.UOMId == uomId);
            if (mapping != null)
            {
                if (!mapping.IsActive)
                {
                    mapping.IsActive = true;
                    mapping.ModifiedDate = now;
                    mapping.ModifiedBy = userId;
                }
            }
            else
            {
                dbContext.VehicleCategoryUoms.Add(new VehicleCategoryUom
                {
                    Id = Guid.NewGuid(),
                    VehicleCategoryId = category.Id,
                    UOMId = uomId,
                    IsActive = true,
                    CreatedDate = now,
                    CreatedBy = userId
                });
            }
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
