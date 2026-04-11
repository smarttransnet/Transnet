using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Trips.Common;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Trips.GetTrips;

internal sealed class GetTripsQueryHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetTripsQuery, List<TripResponse>>
{
    public async Task<Result<List<TripResponse>>> Handle(GetTripsQuery request, CancellationToken cancellationToken)
    {
        List<TripResponse> trips = await dbContext.Trips
            .AsNoTracking()
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
                null, // DriverName
                null, // VehicleRegistrationNumber
                null, // ClientName
                null,
                null,
                null,
                null))
            .ToListAsync(cancellationToken);

        return trips;
    }
}
