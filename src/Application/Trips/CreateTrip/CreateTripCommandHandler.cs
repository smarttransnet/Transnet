using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Trips;
using Domain.Trips.Enums;
using SharedKernel;

namespace Application.Trips.CreateTrip;

internal sealed class CreateTripCommandHandler(
    IApplicationDbContext dbContext,
    IDateTimeProvider dateTimeProvider)
    : ICommandHandler<CreateTripCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateTripCommand request, CancellationToken cancellationToken)
    {
        var trip = new Trip
        {
            Id = Guid.NewGuid(),
            TripNumber = request.TripNumber,
            DriverId = request.DriverId,
            VehicleId = request.VehicleId,
            TrailerId = request.TrailerId,
            Status = TripStatus.Scheduled,
            ScheduledStartAt = request.ScheduledStartAt,
            CreatedAt = dateTimeProvider.UtcNow,
            UpdatedAt = dateTimeProvider.UtcNow,
            IsImported = false
        };

        dbContext.Trips.Add(trip);

        await dbContext.SaveChangesAsync(cancellationToken);

        return trip.Id;
    }
}
