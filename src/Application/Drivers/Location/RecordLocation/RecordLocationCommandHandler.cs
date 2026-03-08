using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Drivers;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Drivers.Location.RecordLocation;

internal sealed class RecordLocationCommandHandler(
    IApplicationDbContext dbContext,
    IDateTimeProvider dateTimeProvider) : ICommandHandler<RecordLocationCommand, Guid>
{
    public async Task<Result<Guid>> Handle(RecordLocationCommand request, CancellationToken cancellationToken)
    {
        bool driverExists = await dbContext.Drivers.AnyAsync(d => d.Id == request.DriverId, cancellationToken);
        if (!driverExists)
        {
            return Result.Failure<Guid>(DriverErrors.NotFound(request.DriverId));
        }

        DateTime utcNow = dateTimeProvider.UtcNow;

        DriverGpsLog? activeGpsSession = await dbContext.DriverGpsLogs
            .FirstOrDefaultAsync(g => g.DriverId == request.DriverId && g.SessionEnd == null, cancellationToken);

        var locationUpdate = new DriverLocationUpdate
        {
            Id = Guid.NewGuid(),
            DriverId = request.DriverId,
            TripId = request.TripId,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            Accuracy = request.Accuracy,
            SpeedKmh = request.SpeedKmh,
            Bearing = request.Bearing,
            RecordedAt = utcNow,
            Source = request.Source
        };

        if (activeGpsSession != null)
        {
            activeGpsSession.PointCount++;
            if (request.TripId.HasValue && activeGpsSession.TripId == null)
            {
                activeGpsSession.TripId = request.TripId;
            }

            if (request.SpeedKmh.HasValue && (activeGpsSession.MaxSpeedKmh == null || request.SpeedKmh > activeGpsSession.MaxSpeedKmh))
            {
                activeGpsSession.MaxSpeedKmh = request.SpeedKmh;
            }
        }

        dbContext.DriverLocationUpdates.Add(locationUpdate);
        await dbContext.SaveChangesAsync(cancellationToken);

        return locationUpdate.Id;
    }
}
