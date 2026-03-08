using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.AssetLocations.GetAssetLocationHistory;

internal sealed class GetAssetLocationHistoryQueryHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetAssetLocationHistoryQuery, List<AssetLocationResponse>>
{
    public async Task<Result<List<AssetLocationResponse>>> Handle(GetAssetLocationHistoryQuery request, CancellationToken cancellationToken)
    {
        List<AssetLocationResponse> locations = await dbContext.AssetLocations
            .AsNoTracking()
            .Where(l => l.AssetType == request.AssetType && l.AssetId == request.AssetId)
            .OrderByDescending(l => l.RecordedAt)
            .Take(50) // Limit history to last 50 entries
            .Select(l => new AssetLocationResponse(
                l.Id,
                l.AssetType,
                l.AssetId,
                l.Latitude,
                l.Longitude,
                l.LocationName,
                l.IsAssigned,
                l.Source,
                l.RecordedAt))
            .ToListAsync(cancellationToken);

        return locations;
    }
}
