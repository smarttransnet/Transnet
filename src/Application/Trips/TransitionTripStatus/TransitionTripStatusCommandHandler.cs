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

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            // Reload and retry once
            if (_context is DbContext dbContext)
            {
                dbContext.Entry(trip!).State = EntityState.Detached;
            }
            
            Trip? updatedTrip = await _context.Trips
                .Include(t => t.StatusHistory)
                .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

            if (updatedTrip is null)
            {
                return Result.Failure(TripErrors.NotFound(request.Id));
            }

            // Re-apply state changes to the fresh entity
            TripStatus currentStatus = updatedTrip.Status;
            updatedTrip.Status = request.NewStatus;
            updatedTrip.UpdatedAt = DateTime.UtcNow;

            if (request.NewStatus == TripStatus.InProgress && updatedTrip.ActualStartAt is null)
            {
                updatedTrip.ActualStartAt = DateTime.UtcNow;
            }
            else if (request.NewStatus == TripStatus.Completed && updatedTrip.ActualEndAt is null)
            {
                updatedTrip.ActualEndAt = DateTime.UtcNow;
            }

            updatedTrip.StatusHistory.Add(new TripStatusHistory
            {
                Id = Guid.NewGuid(),
                TripId = updatedTrip.Id,
                PreviousStatus = currentStatus,
                NewStatus = request.NewStatus,
                ChangedAt = DateTime.UtcNow,
                Notes = request.Notes,
                Source = request.Source,
                ChangedByUserId = request.ChangedByUserId,
                ChangedByDriverId = request.ChangedByDriverId
            });

            await _context.SaveChangesAsync(cancellationToken);
        }

        return Result.Success();
    }
}
