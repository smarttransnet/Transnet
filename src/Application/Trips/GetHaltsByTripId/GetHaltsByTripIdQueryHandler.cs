using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Trips.Common;
using Domain.Trips;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Trips.GetHaltsByTripId;

internal sealed class GetHaltsByTripIdQueryHandler : IQueryHandler<GetHaltsByTripIdQuery, List<TripHaltResponse>>
{
    private readonly IApplicationDbContext _context;

    public GetHaltsByTripIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<TripHaltResponse>>> Handle(GetHaltsByTripIdQuery request, CancellationToken cancellationToken)
    {
        List<TripHaltResponse> halts = await _context.TripHalts
            .Where(h => h.TripId == request.TripId)
            .OrderBy(h => h.StartedAt)
            .Select(h => new TripHaltResponse(
                h.Id,
                h.TripId,
                h.HaltType,
                h.Reason,
                h.Latitude,
                h.Longitude,
                h.LocationName,
                h.StartedAt,
                h.EndedAt,
                h.DurationMinutes,
                h.RecordedByDriverId))
            .ToListAsync(cancellationToken);

        return Result.Success(halts);
    }
}
