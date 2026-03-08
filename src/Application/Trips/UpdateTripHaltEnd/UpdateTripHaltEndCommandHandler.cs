using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Trips;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Trips.UpdateTripHaltEnd;

internal sealed class UpdateTripHaltEndCommandHandler : ICommandHandler<UpdateTripHaltEndCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateTripHaltEndCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(UpdateTripHaltEndCommand request, CancellationToken cancellationToken)
    {
        TripHalt? halt = await _context.TripHalts
            .FirstOrDefaultAsync(h => h.Id == request.Id && h.TripId == request.TripId, cancellationToken);

        if (halt is null)
        {
            return Result.Failure(Error.NotFound("TripHalt.NotFound", $"Trip halt with ID {request.Id} for trip {request.TripId} was not found."));
        }

        halt.EndedAt = DateTime.SpecifyKind(request.EndedAt, DateTimeKind.Utc);
        halt.DurationMinutes = (int)(halt.EndedAt.Value - halt.StartedAt).TotalMinutes;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
