using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Drivers;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Drivers.Location.GetGpsLogs;

internal sealed class GetGpsLogsQueryHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetGpsLogsQuery, PagedList<GpsLogResponse>>
{
    public async Task<Result<PagedList<GpsLogResponse>>> Handle(GetGpsLogsQuery request, CancellationToken cancellationToken)
    {
        IQueryable<DriverGpsLog> gpsLogsQuery = dbContext.DriverGpsLogs;

        if (request.DriverId.HasValue)
        {
            gpsLogsQuery = gpsLogsQuery.Where(l => l.DriverId == request.DriverId.Value);
        }

        if (request.TripId.HasValue)
        {
            gpsLogsQuery = gpsLogsQuery.Where(l => l.TripId == request.TripId.Value);
        }

        int totalCount = await gpsLogsQuery.CountAsync(cancellationToken);

        List<GpsLogResponse> gpsLogs = await gpsLogsQuery
            .OrderByDescending(l => l.SessionStart)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(l => new GpsLogResponse(
                l.Id,
                l.DriverId,
                l.TripId,
                l.SessionStart,
                l.SessionEnd,
                l.TotalDistanceKm,
                l.MaxSpeedKmh,
                l.PointCount,
                l.RawTrackUrl != null ? new Uri(l.RawTrackUrl) : null))
            .ToListAsync(cancellationToken);

        return new PagedList<GpsLogResponse>(gpsLogs, totalCount, request.Page, request.PageSize);
    }
}
