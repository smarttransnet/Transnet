using Application.Abstractions.Messaging;

namespace Application.Assets.GetVehicles;

public sealed record GetVehicleByIdQuery(Guid VehicleId) : IQuery<VehicleResponse>;
