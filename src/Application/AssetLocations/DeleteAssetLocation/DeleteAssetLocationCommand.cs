using Application.Abstractions.Messaging;

namespace Application.AssetLocations.DeleteAssetLocation;

public sealed record DeleteAssetLocationCommand(Guid Id) : ICommand;
