using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Trips;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Trips.UpdateTrip;

internal sealed class UpdateTripCommandHandler : ICommandHandler<UpdateTripCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateTripCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(UpdateTripCommand request, CancellationToken cancellationToken)
    {
        Trip? trip = await _context.Trips
            .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (trip is null)
        {
            return Result.Failure(TripErrors.NotFound(request.Id));
        }

        trip.DriverId = request.DriverId;
        trip.VehicleId = request.VehicleId;
        trip.TrailerId = request.TrailerId;
        trip.ScheduledStartAt = DateTime.SpecifyKind(request.ScheduledStartAt, DateTimeKind.Utc);
        trip.TotalDistanceKm = request.TotalDistanceKm;
        trip.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
