using Application.Abstractions.Messaging;

namespace Application.Inspections.GetVehicleInspections;

public sealed record GetVehicleInspectionsQuery(Guid VehicleId) : IQuery<List<VehicleInspectionResponse>>;
