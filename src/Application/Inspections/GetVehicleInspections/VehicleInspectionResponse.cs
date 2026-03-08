using Domain.Inspections.Enums;

namespace Application.Inspections.GetVehicleInspections;

public sealed record InspectionResultResponse(
    Guid ChecklistItemId,
    bool IsPassed,
    string? Remarks,
    DateTime RecordedAt);

public sealed record VehicleInspectionResponse(
    Guid Id,
    Guid VehicleId,
    Guid InspectionChecklistId,
    InspectionType InspectionType,
    Guid DriverId,
    Guid? TripId,
    DateTime InspectedAt,
    string? DriverSignature,
    DateTime? DriverSignedAt,
    string? Notes,
    decimal OdometerReading,
    InspectionStatus Status,
    List<InspectionResultResponse> InspectionResults);
