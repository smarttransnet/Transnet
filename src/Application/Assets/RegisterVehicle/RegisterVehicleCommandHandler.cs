using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Assets;
using SharedKernel;

namespace Application.Assets.RegisterVehicle;

internal sealed class RegisterVehicleCommandHandler(
    IApplicationDbContext dbContext,
    IDateTimeProvider dateTimeProvider)
    : ICommandHandler<RegisterVehicleCommand, Guid>
{
    public async Task<Result<Guid>> Handle(RegisterVehicleCommand request, CancellationToken cancellationToken)
    {
        var vehicle = new Vehicle
        {
            Id = Guid.NewGuid(),
            PlateNumber = request.PlateNumber,
            ChassisNumber = request.ChassisNumber,
            Make = request.Make,
            Model = request.Model,
            Year = request.Year,
            VehicleCategoryId = request.VehicleCategoryId,
            VehicleType = request.VehicleType,
            Status = request.Status,
            OdometerReading = request.OdometerReading,
            IsActive = true,
            CreatedAt = dateTimeProvider.UtcNow,
            UpdatedAt = dateTimeProvider.UtcNow
        };

        dbContext.Vehicles.Add(vehicle);

        await dbContext.SaveChangesAsync(cancellationToken);

        return vehicle.Id;
    }
}
