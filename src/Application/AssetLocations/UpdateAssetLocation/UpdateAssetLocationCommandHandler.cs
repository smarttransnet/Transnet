using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Assets;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.AssetLocations.UpdateAssetLocation;

internal sealed class UpdateAssetLocationCommandHandler(IApplicationDbContext dbContext)
    : ICommandHandler<UpdateAssetLocationCommand>
{
    public async Task<Result> Handle(UpdateAssetLocationCommand request, CancellationToken cancellationToken)
    {
        AssetLocation? location = await dbContext.AssetLocations
            .FirstOrDefaultAsync(l => l.Id == request.Id, cancellationToken);

        if (location is null)
        {
            return Result.Failure(Error.NotFound("AssetLocation.NotFound", "The asset location was not found."));
        }

        location.Latitude = request.Latitude;
        location.Longitude = request.Longitude;
        location.LocationName = request.LocationName;

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
