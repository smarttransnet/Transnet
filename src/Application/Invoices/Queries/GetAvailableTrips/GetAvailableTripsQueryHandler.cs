using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Trips.Common;
using Domain.Trips.Enums;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Invoices.Queries.GetAvailableTrips;

internal sealed class GetAvailableTripsQueryHandler(
    IApplicationDbContext dbContext)
    : IQueryHandler<GetAvailableTripsQuery, IReadOnlyList<TripResponse>>
{
    public async Task<Result<IReadOnlyList<TripResponse>>> Handle(GetAvailableTripsQuery request, CancellationToken cancellationToken)
    {
        // Find trips that:
        // 1. Belong to the requested client
        // 2. Are in 'Completed' status (or 'Invoiced' if we want to allow re-invoicing, but usually Completed is better)
        // 3. Are NOT already part of any InvoiceTripLink
        
        var linkedTripIds = await dbContext.InvoiceTripLinks
            .Select(tl => tl.TripId)
            .ToListAsync(cancellationToken);

        List<TripResponse> trips = await dbContext.Trips
            .AsNoTracking()
            .Where(t => t.ClientId == request.ClientId && 
                        t.Status == TripStatus.Completed && 
                        !linkedTripIds.Contains(t.Id))
            .OrderByDescending(t => t.ActualEndAt)
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
                null, // VehicleChassisNumber
                null, // ClientName
                null, // Stops not needed for the selection list
                null, // Halts not needed
                null, // Voucher not needed
                null  // PodUploads not needed
            ))
            .ToListAsync(cancellationToken);

        return trips;
    }
}
