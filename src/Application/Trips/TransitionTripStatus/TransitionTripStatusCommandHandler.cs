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

        ApplyChanges(trip, request);

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            // If the context is a DbContext, detach all tracked entities to avoid interference on retry
            if (_context is DbContext dbContext)
            {
                foreach (var entry in dbContext.ChangeTracker.Entries().ToList())
                {
                    entry.State = EntityState.Detached;
                }
            }

            // Reload and retry once
            Trip? updatedTrip = await _context.Trips
                .Include(t => t.StatusHistory)
                .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

            if (updatedTrip is null)
            {
                return Result.Failure(TripErrors.NotFound(request.Id));
            }

            ApplyChanges(updatedTrip, request);

            await _context.SaveChangesAsync(cancellationToken);
        }

        return Result.Success();
    }

    private static void ApplyChanges(Trip trip, TransitionTripStatusCommand request)
    {
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

        string? attachmentUrl = null;
        if (request.PhotoStream is not null && !string.IsNullOrWhiteSpace(request.PhotoFileName))
        {
            // In a real app, upload to blob storage
            attachmentUrl = $"/uploads/trips/{trip.Id}/status/{request.PhotoFileName}";
        }

        // Record history
        trip.StatusHistory.Add(new TripStatusHistory
        {
            TripId = trip.Id,
            PreviousStatus = previousStatus,
            NewStatus = request.NewStatus,
            ChangedAt = DateTime.UtcNow,
            Notes = request.Notes,
            AttachmentUrl = attachmentUrl,
            Source = request.Source,
            ChangedByUserId = request.ChangedByUserId,
            ChangedByDriverId = request.ChangedByDriverId
        });
    }
}
