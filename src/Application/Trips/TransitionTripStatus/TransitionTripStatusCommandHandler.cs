using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Trips;
using Domain.Trips.Enums;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Trips.TransitionTripStatus;

internal sealed class TransitionTripStatusCommandHandler : ICommandHandler<TransitionTripStatusCommand>
{
    private readonly IApplicationDbContext _context;

    public TransitionTripStatusCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(TransitionTripStatusCommand request, CancellationToken cancellationToken)
    {
        Trip? trip = await _context.Trips
            .Include(t => t.StatusHistory)
            .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (trip is null)
        {
            return Result.Failure(TripErrors.NotFound(request.Id));
        }

        TripStatus previousStatus = trip.Status;
        trip.Status = request.NewStatus;
        trip.UpdatedAt = DateTime.UtcNow;

        // Update timestamps based on status
        if (request.NewStatus == TripStatus.InProgress && trip.ActualStartAt is null)
        {
            trip.ActualStartAt = DateTime.UtcNow;
        }
        else if (request.NewStatus == TripStatus.Completed && trip.ActualEndAt is null)
        {
            trip.ActualEndAt = DateTime.UtcNow;
        }

        // Record history
        trip.StatusHistory.Add(new TripStatusHistory
        {
            Id = Guid.NewGuid(),
            TripId = trip.Id,
            PreviousStatus = previousStatus,
            NewStatus = request.NewStatus,
            ChangedAt = DateTime.UtcNow,
            Notes = request.Notes,
            Source = request.Source,
            ChangedByUserId = request.ChangedByUserId,
            ChangedByDriverId = request.ChangedByDriverId
        });

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
