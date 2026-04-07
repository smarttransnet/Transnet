using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Trips;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Trips.DeleteTripHalt;

internal sealed class DeleteTripHaltCommandHandler : ICommandHandler<DeleteTripHaltCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteTripHaltCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(DeleteTripHaltCommand request, CancellationToken cancellationToken)
    {
        TripHalt? halt = await _context.TripHalts
            .FirstOrDefaultAsync(h => h.Id == request.HaltId && h.TripId == request.TripId, cancellationToken);

        if (halt is null)
        {
            return Result.Failure(Error.NotFound("TripHalt.NotFound", $"Trip halt with ID {request.HaltId} for trip {request.TripId} was not found."));
        }

        _context.TripHalts.Remove(halt);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
