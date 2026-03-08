using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Trips;
using Domain.Trips.Enums;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Trips.AddTripStop;

internal sealed class AddTripStopCommandHandler : ICommandHandler<AddTripStopCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public AddTripStopCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Guid>> Handle(AddTripStopCommand request, CancellationToken cancellationToken)
    {
        bool tripExists = await _context.Trips.AnyAsync(t => t.Id == request.TripId, cancellationToken);
        if (!tripExists)
        {
            return Result.Failure<Guid>(TripErrors.NotFound(request.TripId));
        }

        TripStop stop = new()
        {
            Id = Guid.NewGuid(),
            TripId = request.TripId,
            StopOrder = request.StopOrder,
            StopType = request.StopType,
            LocationName = request.LocationName,
            Address = request.Address,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            PocName = request.PocName,
            PocPhone = request.PocPhone,
            PocEmail = request.PocEmail,
            ScheduledArrivalAt = request.ScheduledArrivalAt.HasValue
                ? DateTime.SpecifyKind(request.ScheduledArrivalAt.Value, DateTimeKind.Utc)
                : null,
            Notes = request.Notes
        };

        _context.TripStops.Add(stop);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(stop.Id);
    }
}
