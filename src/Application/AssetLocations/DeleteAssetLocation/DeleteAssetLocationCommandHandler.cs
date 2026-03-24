using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Assets;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.AssetLocations.DeleteAssetLocation;

internal sealed class DeleteAssetLocationCommandHandler(IApplicationDbContext dbContext)
    : ICommandHandler<DeleteAssetLocationCommand>
{
    public async Task<Result> Handle(DeleteAssetLocationCommand request, CancellationToken cancellationToken)
    {
        AssetLocation? location = await dbContext.AssetLocations
            .FirstOrDefaultAsync(l => l.Id == request.Id, cancellationToken);

        if (location is null)
        {
            return Result.Failure(Error.NotFound("AssetLocation.NotFound", "The asset location was not found."));
        }

        dbContext.AssetLocations.Remove(location);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
