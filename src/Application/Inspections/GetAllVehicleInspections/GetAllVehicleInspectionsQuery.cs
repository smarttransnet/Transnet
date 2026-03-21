using Application.Abstractions.Messaging;

namespace Application.Inspections.GetAllVehicleInspections;

public sealed record GetAllVehicleInspectionsQuery : IQuery<List<VehicleInspectionResponse>>;
