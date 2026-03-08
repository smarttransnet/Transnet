using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Assets;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Trailers.AttachTrailer;

internal sealed class AttachTrailerCommandHandler(
    IApplicationDbContext dbContext,
    IDateTimeProvider dateTimeProvider)
    : ICommandHandler<AttachTrailerCommand>
{
    public async Task<Result> Handle(AttachTrailerCommand request, CancellationToken cancellationToken)
    {
        Trailer? trailer = await dbContext.Trailers
            .FirstOrDefaultAsync(t => t.Id == request.TrailerId, cancellationToken);

        if (trailer is null)
        {
            return Result.Failure(TrailerErrors.NotFound(request.TrailerId));
        }

        // Technically we should check if the vehicle exists as well.
        bool vehicleExists = await dbContext.Vehicles
            .AnyAsync(v => v.Id == request.VehicleId, cancellationToken);

        if (!vehicleExists)
        {
            return Result.Failure(Error.NotFound("Vehicles.NotFound", $"The vehicle with the Id = '{request.VehicleId}' was not found"));
        }

        trailer.AttachedVehicleId = request.VehicleId;
        trailer.UpdatedAt = dateTimeProvider.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
