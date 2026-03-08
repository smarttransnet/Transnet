using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Trips;
using Domain.Trips.Enums;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Trips.RecordTripHalt;

internal sealed class RecordTripHaltCommandHandler : ICommandHandler<RecordTripHaltCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public RecordTripHaltCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Guid>> Handle(RecordTripHaltCommand request, CancellationToken cancellationToken)
    {
        bool tripExists = await _context.Trips.AnyAsync(t => t.Id == request.TripId, cancellationToken);
        if (!tripExists)
        {
            return Result.Failure<Guid>(TripErrors.NotFound(request.TripId));
        }

        TripHalt halt = new()
        {
            Id = Guid.NewGuid(),
            TripId = request.TripId,
            HaltType = request.HaltType,
            Reason = request.Reason,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            LocationName = request.LocationName,
            StartedAt = DateTime.SpecifyKind(request.StartedAt, DateTimeKind.Utc),
            RecordedByDriverId = request.RecordedByDriverId
        };

        _context.TripHalts.Add(halt);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(halt.Id);
    }
}
