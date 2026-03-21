using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Assets;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Assets.DeleteVehicle;

internal sealed class DeleteVehicleCommandHandler(IApplicationDbContext context)
    : ICommandHandler<DeleteVehicleCommand>
{
    public async Task<Result> Handle(DeleteVehicleCommand command, CancellationToken cancellationToken)
    {
        Vehicle? vehicle = await context.Vehicles
            .FirstOrDefaultAsync(v => v.Id == command.Id, cancellationToken);

        if (vehicle is null)
        {
            return Result.Failure(Error.NotFound("Vehicles.NotFound", $"The vehicle with the Id = '{command.Id}' was not found"));
        }

        context.Vehicles.Remove(vehicle);

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
