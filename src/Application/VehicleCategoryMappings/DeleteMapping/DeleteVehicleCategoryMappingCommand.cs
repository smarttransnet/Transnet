using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.VehicleCategoryMappings.DeleteMapping;

public sealed record DeleteVehicleCategoryMappingCommand(Guid VehicleCategoryId) : ICommand;

internal sealed class DeleteVehicleCategoryMappingCommandHandler(
    IApplicationDbContext dbContext,
    IDateTimeProvider dateTimeProvider,
    IUserContext userContext
) : ICommandHandler<DeleteVehicleCategoryMappingCommand>
{
    public async Task<Result> Handle(
        DeleteVehicleCategoryMappingCommand request,
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

        // We soft delete all mappings for this category
        var mappings = await dbContext.VehicleCategoryUoms
            .Where(m => m.VehicleCategoryId == request.VehicleCategoryId && m.IsActive)
            .ToListAsync(cancellationToken);

        foreach (var mapping in mappings)
        {
            mapping.IsActive = false;
            mapping.ModifiedDate = now;
            mapping.ModifiedBy = userId;
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
