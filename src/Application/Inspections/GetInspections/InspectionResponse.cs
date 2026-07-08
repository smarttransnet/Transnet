using Domain.Inspections.Enums;

namespace Application.Inspections.GetInspections;

public sealed record InspectionResponse(
    Guid Id,
    Guid VehicleId,
    string VehicleChassisNumber,
    string ChecklistName,
    InspectionType InspectionType,
    DateTime InspectedAt,
    InspectionStatus Status);
