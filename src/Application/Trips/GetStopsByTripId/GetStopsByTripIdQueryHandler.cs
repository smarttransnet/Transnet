using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Trips.Common;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Trips.GetStopsByTripId;

internal sealed class GetStopsByTripIdQueryHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetStopsByTripIdQuery, List<TripStopResponse>>
{
    public async Task<Result<List<TripStopResponse>>> Handle(GetStopsByTripIdQuery request, CancellationToken cancellationToken)
    {
        List<TripStopResponse> stops = await dbContext.TripStops
            .AsNoTracking()
            .Where(s => s.TripId == request.TripId)
            .OrderBy(s => s.StopOrder)
            .Select(s => new TripStopResponse(
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
                s.Notes))
            .ToListAsync(cancellationToken);

        return stops;
    }
}
