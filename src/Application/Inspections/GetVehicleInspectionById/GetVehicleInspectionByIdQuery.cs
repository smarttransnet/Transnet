using Application.Abstractions.Messaging;

namespace Application.Inspections.GetVehicleInspectionById;

public sealed record GetVehicleInspectionByIdQuery(Guid Id) : IQuery<VehicleInspectionResponse>;
