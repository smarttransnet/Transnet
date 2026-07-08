using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Assets;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Assets.UpdateVehicle;

internal sealed class UpdateVehicleCommandHandler(
    IApplicationDbContext dbContext,
    IDateTimeProvider dateTimeProvider)
    : ICommandHandler<UpdateVehicleCommand>
{
    public async Task<Result> Handle(UpdateVehicleCommand request, CancellationToken cancellationToken)
    {
        Vehicle? vehicle = await dbContext.Vehicles
            .FirstOrDefaultAsync(v => v.Id == request.VehicleId, cancellationToken);

        if (vehicle is null)
        {
            return Result.Failure(VehicleErrors.NotFound(request.VehicleId));
        }

        vehicle.PlateNumber = request.PlateNumber;
        vehicle.ChassisNumber = request.ChassisNumber;
        vehicle.Make = request.Make;
        vehicle.Model = request.Model;
        vehicle.Year = request.Year;
        vehicle.VehicleCategoryId = request.VehicleCategoryId;
        vehicle.VehicleType = request.VehicleType;
        vehicle.Status = request.Status;
        vehicle.OdometerReading = request.OdometerReading;
        vehicle.IsActive = request.IsActive;
        vehicle.UpdatedAt = dateTimeProvider.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
