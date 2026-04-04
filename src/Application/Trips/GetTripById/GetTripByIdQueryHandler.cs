using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Trips.Common;
using Domain.Trips;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Trips.GetTripById;

internal sealed class GetTripByIdQueryHandler : IQueryHandler<GetTripByIdQuery, TripResponse>
{
    private readonly IApplicationDbContext _context;

    public GetTripByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<TripResponse>> Handle(GetTripByIdQuery request, CancellationToken cancellationToken)
    {
        Trip? trip = await _context.Trips
            .Include(t => t.Stops)
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (trip is null)
        {
            return Result.Failure<TripResponse>(TripErrors.NotFound(request.Id));
        }

        TripResponse response = new(
            trip.Id,
            trip.TripNumber,
            trip.DriverId,
            trip.VehicleId,
            trip.TrailerId,
            trip.Status,
            trip.ScheduledStartAt,
            trip.ActualStartAt,
            trip.ActualEndAt,
            trip.TotalDistanceKm,
            trip.IsImported,
            trip.ImportBatchId,
            trip.Origin,
            trip.Destination,
            trip.DriverConfirmedAt,
            trip.OfficeApprovedAt,
            trip.OfficeApprovedByUserId,
            trip.CreatedAt,
            trip.UpdatedAt,
            trip.Stops.OrderBy(s => s.StopOrder).Select(s => new TripStopResponse(
                s.Id,
                s.TripId,
                s.StopOrder,
                s.StopType,
                s.LocationName,
                s.Address,
                s.Latitude,
                s.Longitude,
                s.PocName,
                s.PocPhone,
                s.PocEmail,
                s.ScheduledArrivalAt,
                s.ActualArrivalAt,
                s.ActualDepartureAt,
                s.Notes)).ToList());

        return Result.Success(response);
    }
}
