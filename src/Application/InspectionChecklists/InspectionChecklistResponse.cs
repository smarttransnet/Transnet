using Domain.Inspections.Enums;

namespace Application.InspectionChecklists;

public sealed record ChecklistItemResponse(
    Guid Id,
    string ItemName,
    string Category,
    bool IsRequired,
    int SortOrder);

public sealed record InspectionChecklistResponse(
    Guid Id,
    string Name,
    InspectionType InspectionType,
    string ApplicableVehicleTypes,
    bool IsActive,
    List<ChecklistItemResponse> Items);
