using Application.Abstractions.Messaging;
using Domain.WorkOrders.Enums;

namespace Application.AssetLocations.GetAssetLocationHistory;

public sealed record GetAssetLocationHistoryQuery(
    AssetType AssetType,
    Guid AssetId) : IQuery<List<AssetLocationResponse>>;
