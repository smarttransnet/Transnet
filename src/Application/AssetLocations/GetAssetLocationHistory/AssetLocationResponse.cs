using Domain.Assets.Enums;
using Domain.WorkOrders.Enums;

namespace Application.AssetLocations.GetAssetLocationHistory;

public sealed record AssetLocationResponse(
    Guid Id,
    AssetType AssetType,
    Guid AssetId,
    double Latitude,
    double Longitude,
    string? LocationName,
    bool IsAssigned,
    LocationSource Source,
    DateTime RecordedAt);
