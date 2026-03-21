using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Assets;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.VehicleCategories.DeleteVehicleCategory;

internal sealed class DeleteVehicleCategoryCommandHandler(IApplicationDbContext context)
    : ICommandHandler<DeleteVehicleCategoryCommand>
{
    public async Task<Result> Handle(DeleteVehicleCategoryCommand command, CancellationToken cancellationToken)
    {
        VehicleCategory? vehicleCategory = await context.VehicleCategories
            .FirstOrDefaultAsync(v => v.Id == command.Id, cancellationToken);

        if (vehicleCategory is null)
        {
            return Result.Failure(Error.NotFound("VehicleCategories.NotFound", $"The vehicle category with the Id = '{command.Id}' was not found"));
        }

        context.VehicleCategories.Remove(vehicleCategory);

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
