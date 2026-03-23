using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Assets;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Vehicles.DeleteVehicle;

internal sealed class DeleteVehicleCommandHandler(IApplicationDbContext dbContext)
    : ICommandHandler<DeleteVehicleCommand>
{
    public async Task<Result> Handle(DeleteVehicleCommand request, CancellationToken cancellationToken)
    {
        Vehicle? vehicle = await dbContext.Vehicles
            .FirstOrDefaultAsync(v => v.Id == request.Id, cancellationToken);

        if (vehicle is null)
        {
            return Result.Failure(Error.NotFound("Vehicle.NotFound", "The vehicle was not found."));
        }

        dbContext.Vehicles.Remove(vehicle);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
