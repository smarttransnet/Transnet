using Application.Abstractions.Messaging;
using Domain.Inspections.Enums;

namespace Application.Inspections.SubmitInspection;

public sealed record SubmitInspectionResultCommand(
    Guid ChecklistItemId,
    string Status,
    string? Remarks);

public sealed record SubmitInspectionCommand(
    Guid VehicleId,
    Guid InspectionChecklistId,
    InspectionType InspectionType,
    Guid DriverId,
    Guid? TripId,
    string? DriverSignature,
    string? Notes,
    decimal OdometerReading,
    List<SubmitInspectionResultCommand> InspectionResults) : ICommand<Guid>;
