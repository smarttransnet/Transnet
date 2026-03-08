using Application.Abstractions.Messaging;

namespace Application.Assets.GetVehicles;

public sealed record GetVehiclesQuery() : IQuery<List<VehicleResponse>>;
