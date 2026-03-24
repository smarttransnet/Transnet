using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Assets;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Trailers.UpdateTrailer;

internal sealed class UpdateTrailerCommandHandler(IApplicationDbContext dbContext)
    : ICommandHandler<UpdateTrailerCommand>
{
    public async Task<Result> Handle(UpdateTrailerCommand request, CancellationToken cancellationToken)
    {
        Trailer? trailer = await dbContext.Trailers
            .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (trailer is null)
        {
            return Result.Failure(Error.NotFound("Trailer.NotFound", "The trailer was not found."));
        }

        trailer.TrailerNumber = request.TrailerNumber;
        trailer.TrailerType = request.TrailerType;
        trailer.Capacity = request.Capacity;
        trailer.CapacityUnit = request.CapacityUnit;
        trailer.IsActive = request.IsActive;
        trailer.UpdatedAt = DateTime.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
