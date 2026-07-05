using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Trips;
using Domain.Trips.Enums;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Trips.CreateTrip;

internal sealed class CreateTripCommandHandler(
    IApplicationDbContext dbContext,
    IDateTimeProvider dateTimeProvider)
    : ICommandHandler<CreateTripCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateTripCommand request, CancellationToken cancellationToken)
    {
        if (request.VehicleCategoryUomId.HasValue)
        {
            var mappingExists = await dbContext.VehicleCategoryUoms
                .AnyAsync(m => m.Id == request.VehicleCategoryUomId.Value && m.IsActive, cancellationToken);
            if (!mappingExists)
            {
                return Result.Failure<Guid>(Error.NotFound(
                    "VehicleCategoryUom.NotFound",
                    $"Vehicle category UOM mapping with ID '{request.VehicleCategoryUomId}' was not found."
                ));
            }

            if (!request.Quantity.HasValue || request.Quantity.Value <= 0)
            {
                return Result.Failure<Guid>(Error.Problem(
                    "Trip.QuantityRequired",
                    "Quantity is required and must be greater than 0 when Vehicle Category and UOM are selected."
                ));
            }
        }

        var trip = new Trip
        {
            Id = Guid.NewGuid(),
            TripNumber = request.TripNumber,
            DriverId = request.DriverId,
            VehicleId = request.VehicleId,
            TrailerId = request.TrailerId,
            ClientId = request.ClientId,
            Origin = request.Origin ?? string.Empty,
            Destination = request.Destination ?? string.Empty,
            Status = TripStatus.Scheduled,
            ScheduledStartAt = request.ScheduledStartAt,
            CreatedAt = dateTimeProvider.UtcNow,
            UpdatedAt = dateTimeProvider.UtcNow,
            IsImported = false,
            VehicleCategoryUomId = request.VehicleCategoryUomId,
            Quantity = request.Quantity
        };

        dbContext.Trips.Add(trip);

        await dbContext.SaveChangesAsync(cancellationToken);

        return trip.Id;
    }
}
