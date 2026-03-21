using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Trailers;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Trailers.UpdateTrailer;

internal sealed class UpdateTrailerCommandHandler(IApplicationDbContext dbContext, IDateTimeProvider dateTimeProvider)
    : ICommandHandler<UpdateTrailerCommand>
{
    public async Task<Result> Handle(UpdateTrailerCommand request, CancellationToken cancellationToken)
    {
        var trailer = await dbContext.Trailers
            .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (trailer is null)
        {
            return Result.Failure(TrailerErrors.NotFound(request.Id));
        }

        trailer.TrailerNumber = request.TrailerNumber;
        trailer.TrailerType = request.TrailerType;
        trailer.Capacity = request.Capacity;
        trailer.CapacityUnit = request.CapacityUnit;
        trailer.IsActive = request.IsActive;
        trailer.UpdatedAt = dateTimeProvider.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
