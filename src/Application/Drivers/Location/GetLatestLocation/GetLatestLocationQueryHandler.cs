using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Drivers;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Drivers.Location.GetLatestLocation;

internal sealed class GetLatestLocationQueryHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetLatestLocationQuery, LocationResponse>
{
    public async Task<Result<LocationResponse>> Handle(GetLatestLocationQuery request, CancellationToken cancellationToken)
    {
        LocationResponse? location = await dbContext.DriverLocationUpdates
            .AsNoTracking()
            .Where(l => l.DriverId == request.DriverId)
            .OrderByDescending(l => l.RecordedAt)
            .Select(l => new LocationResponse(
                l.Id,
                l.DriverId,
                l.TripId,
                l.Latitude,
                l.Longitude,
                l.Accuracy,
                l.SpeedKmh,
                l.Bearing,
                l.RecordedAt,
                l.Source))
            .FirstOrDefaultAsync(cancellationToken);

        if (location is null)
        {
            return Result.Failure<LocationResponse>(Error.NotFound("Drivers.LocationNotFound", $"Latest location for driver with Id = '{request.DriverId}' was not found"));
        }

        return location;
    }
}
