using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Trips.Common;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Trips.GetPortalJobs;

internal sealed class GetPortalJobsQueryHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetPortalJobsQuery, List<TripResponse>>
{
    public async Task<Result<List<TripResponse>>> Handle(GetPortalJobsQuery request, CancellationToken cancellationToken)
    {
        List<TripResponse> trips = await dbContext.Trips
            .AsNoTracking()
            .Where(t => t.ClientId == request.ClientId)
            .OrderByDescending(t => t.ScheduledStartAt)
            .Select(t => new TripResponse(
                t.Id,
                t.TripNumber,
                t.DriverId,
                t.VehicleId,
                t.TrailerId,
                t.Status,
                t.ScheduledStartAt,
                t.ActualStartAt,
                t.ActualEndAt,
                t.TotalDistanceKm,
                t.IsImported,
                t.ImportBatchId,
                t.Origin,
                t.Destination,
                t.DriverConfirmedAt,
                t.OfficeApprovedAt,
                t.OfficeApprovedByUserId,
                t.CreatedAt,
                t.UpdatedAt,
                null))
            .ToListAsync(cancellationToken);

        return trips;
    }
}
