using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Trips;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Trips.UpdateTripStop;

internal sealed class UpdateTripStopCommandHandler : ICommandHandler<UpdateTripStopCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateTripStopCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(UpdateTripStopCommand request, CancellationToken cancellationToken)
    {
        TripStop? stop = await _context.TripStops
            .FirstOrDefaultAsync(s => s.Id == request.Id && s.TripId == request.TripId, cancellationToken);

        if (stop is null)
        {
            return Result.Failure(Error.NotFound("TripStop.NotFound", $"Trip stop with ID {request.Id} for trip {request.TripId} was not found."));
        }

        stop.StopOrder = request.StopOrder;
        stop.StopType = request.StopType;
        stop.LocationName = request.LocationName;
        stop.Address = request.Address;
        stop.Latitude = request.Latitude;
        stop.Longitude = request.Longitude;
        stop.PocName = request.PocName;
        stop.PocPhone = request.PocPhone;
        stop.PocEmail = request.PocEmail;
        stop.ScheduledArrivalAt = request.ScheduledArrivalAt.HasValue
            ? DateTime.SpecifyKind(request.ScheduledArrivalAt.Value, DateTimeKind.Utc)
            : null;
        stop.ActualArrivalAt = request.ActualArrivalAt.HasValue
            ? DateTime.SpecifyKind(request.ActualArrivalAt.Value, DateTimeKind.Utc)
            : null;
        stop.ActualDepartureAt = request.ActualDepartureAt.HasValue
            ? DateTime.SpecifyKind(request.ActualDepartureAt.Value, DateTimeKind.Utc)
            : null;
        stop.Notes = request.Notes;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
