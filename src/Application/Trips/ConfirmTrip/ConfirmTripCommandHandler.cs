using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Trips;
using Domain.Trips.Enums;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Trips.ConfirmTrip;

internal sealed class ConfirmTripCommandHandler : ICommandHandler<ConfirmTripCommand>
{
    private readonly IApplicationDbContext _context;

    public ConfirmTripCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(ConfirmTripCommand request, CancellationToken cancellationToken)
    {
        Trip? trip = await _context.Trips
            .Include(t => t.StatusHistory)
            .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (trip is null)
        {
            return Result.Failure(TripErrors.NotFound(request.Id));
        }

        if (trip.Status != TripStatus.Completed && trip.Status != TripStatus.PendingDriverConfirmation)
        {
            return Result.Failure(Error.Problem("Trip.InvalidStatus", "Trip must be in Completed or PendingDriverConfirmation status to be confirmed."));
        }

        TripStatus previousStatus = trip.Status;
        trip.Status = TripStatus.PendingOfficeApproval;
        trip.DriverConfirmedAt = DateTime.UtcNow;
        trip.UpdatedAt = DateTime.UtcNow;

        trip.StatusHistory.Add(new TripStatusHistory
        {
            Id = Guid.NewGuid(),
            TripId = trip.Id,
            PreviousStatus = previousStatus,
            NewStatus = TripStatus.PendingOfficeApproval,
            ChangedAt = DateTime.UtcNow,
            Notes = "Driver confirmed trip completion.",
            Source = StatusChangeSource.DriverApp,
            ChangedByDriverId = request.DriverId
        });

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
