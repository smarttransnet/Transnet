using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Assets;
using SharedKernel;

namespace Application.AssetLocations.LogAssetLocation;

internal sealed class LogAssetLocationCommandHandler(
    IApplicationDbContext dbContext,
    IDateTimeProvider dateTimeProvider)
    : ICommandHandler<LogAssetLocationCommand, Guid>
{
    public async Task<Result<Guid>> Handle(LogAssetLocationCommand request, CancellationToken cancellationToken)
    {
        DateTime now = dateTimeProvider.UtcNow;
        var location = new AssetLocation
        {
            Id = Guid.NewGuid(),
            AssetType = request.AssetType,
            AssetId = request.AssetId,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            LocationName = request.LocationName,
            IsAssigned = true,
            Source = request.Source,
            RecordedAt = now
        };

        dbContext.AssetLocations.Add(location);

        await dbContext.SaveChangesAsync(cancellationToken);

        return location.Id;
    }
}
