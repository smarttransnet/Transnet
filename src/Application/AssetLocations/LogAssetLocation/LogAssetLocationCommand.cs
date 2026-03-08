using Application.Abstractions.Messaging;
using Domain.Assets.Enums;
using Domain.WorkOrders.Enums;

namespace Application.AssetLocations.LogAssetLocation;

public sealed record LogAssetLocationCommand(
    AssetType AssetType,
    Guid AssetId,
    double Latitude,
    double Longitude,
    string? LocationName,
    LocationSource Source) : ICommand<Guid>;
