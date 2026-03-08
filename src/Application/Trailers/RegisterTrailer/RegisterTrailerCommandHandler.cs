using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Assets;
using SharedKernel;

namespace Application.Trailers.RegisterTrailer;

internal sealed class RegisterTrailerCommandHandler(
    IApplicationDbContext dbContext,
    IDateTimeProvider dateTimeProvider)
    : ICommandHandler<RegisterTrailerCommand, Guid>
{
    public async Task<Result<Guid>> Handle(RegisterTrailerCommand request, CancellationToken cancellationToken)
    {
        var trailer = new Trailer
        {
            Id = Guid.NewGuid(),
            TrailerNumber = request.TrailerNumber,
            TrailerType = request.TrailerType,
            Capacity = request.Capacity,
            CapacityUnit = request.CapacityUnit,
            Status = request.Status,
            IsActive = true,
            CreatedAt = dateTimeProvider.UtcNow,
            UpdatedAt = dateTimeProvider.UtcNow
        };

        dbContext.Trailers.Add(trailer);

        await dbContext.SaveChangesAsync(cancellationToken);

        return trailer.Id;
    }
}
