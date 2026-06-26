using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Trips;
using Domain.Trips.Enums;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Trips.ApproveTrip;

internal sealed class ApproveTripCommandHandler : ICommandHandler<ApproveTripCommand>
{
    private readonly IApplicationDbContext _context;

    public ApproveTripCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(ApproveTripCommand request, CancellationToken cancellationToken)
    {
        Trip? trip = await _context.Trips
            .Include(t => t.StatusHistory)
            .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (trip is null)
        {
            return Result.Failure(TripErrors.NotFound(request.Id));
        }

        if (trip.Status != TripStatus.PendingOfficeApproval)
        {
            return Result.Failure(Error.Problem("Trip.InvalidStatus", "Trip must be in PendingOfficeApproval status to be approved."));
        }

        TripStatus previousStatus = trip.Status;
        trip.Status = TripStatus.Completed;
        trip.ActualEndAt ??= DateTime.UtcNow;
        trip.OfficeApprovedAt = DateTime.UtcNow;
        trip.OfficeApprovedByUserId = request.UserId;
        trip.UpdatedAt = DateTime.UtcNow;

        trip.StatusHistory.Add(new TripStatusHistory
        {
            TripId = trip.Id,
            PreviousStatus = previousStatus,
            NewStatus = TripStatus.Completed,
            ChangedAt = DateTime.UtcNow,
            Notes = "Office approved trip.",
            Source = StatusChangeSource.OfficePortal,
            ChangedByUserId = request.UserId
        });

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
