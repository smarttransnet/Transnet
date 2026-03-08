using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Trips;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Trips.DeleteTripStop;

internal sealed class DeleteTripStopCommandHandler : ICommandHandler<DeleteTripStopCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteTripStopCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(DeleteTripStopCommand request, CancellationToken cancellationToken)
    {
        TripStop? stop = await _context.TripStops
            .FirstOrDefaultAsync(s => s.Id == request.Id && s.TripId == request.TripId, cancellationToken);

        if (stop is null)
        {
            return Result.Failure(Error.NotFound("TripStop.NotFound", $"Trip stop with ID {request.Id} for trip {request.TripId} was not found."));
        }

        _context.TripStops.Remove(stop);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
