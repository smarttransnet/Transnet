using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Trips;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Trips.UpdateTripHalt;

internal sealed class UpdateTripHaltCommandHandler(
    IApplicationDbContext dbContext)
    : ICommandHandler<UpdateTripHaltCommand>
{
    public async Task<Result> Handle(UpdateTripHaltCommand request, CancellationToken cancellationToken)
    {
        TripHalt? halt = await dbContext.TripHalts
            .FirstOrDefaultAsync(h => h.Id == request.HaltId && h.TripId == request.TripId, cancellationToken);

        if (halt is null)
        {
            return Result.Failure(Error.NotFound("TripHalt.NotFound", "The specified trip halt record was not found."));
        }

        halt.HaltType = request.HaltType;
        halt.Reason = request.Reason;
        halt.Latitude = request.Latitude;
        halt.Longitude = request.Longitude;
        halt.LocationName = request.LocationName;
        halt.StartedAt = DateTime.SpecifyKind(request.StartedAt, DateTimeKind.Utc);
        halt.EndedAt = request.EndedAt.HasValue ? DateTime.SpecifyKind(request.EndedAt.Value, DateTimeKind.Utc) : null;

        halt.DurationMinutes = halt.EndedAt.HasValue
            ? (int)(halt.EndedAt.Value - halt.StartedAt).TotalMinutes
            : null;

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
