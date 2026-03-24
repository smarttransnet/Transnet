using Application.Abstractions.Messaging;

namespace Application.AssetLocations.UpdateAssetLocation;

public sealed record UpdateAssetLocationCommand(
    Guid Id,
    double Latitude,
    double Longitude,
    string? LocationName) : ICommand;
