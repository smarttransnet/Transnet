using System.Diagnostics.CodeAnalysis;
using Domain.Inspections.Enums;

namespace Application.Inspections.GetInspectionById;

public sealed record InspectionResultResponse(
    Guid Id,
    Guid ChecklistItemId,
    string ItemName,
    bool IsPassed,
    string? Remarks);

public sealed record InspectionPhotoResponse(
    Guid Id,
    string PhotoPath,
    string? Caption,
    PhotoType PhotoType,
    DateTime UploadedAt);

public sealed record InspectionDetailedResponse(
    Guid Id,
    Guid VehicleId,
    string VehicleRegistration,
    Guid InspectionChecklistId,
    string ChecklistName,
    InspectionType InspectionType,
    Guid DriverId,
    string DriverName,
    DateTime InspectedAt,
    string? DriverSignature,
    DateTime? DriverSignedAt,
    string? Notes,
    decimal OdometerReading,
    InspectionStatus Status,
    List<InspectionResultResponse> Results,
    List<InspectionPhotoResponse> Photos);
