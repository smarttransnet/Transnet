using Application.Abstractions.Messaging;
using Domain.Inspections.Enums;

namespace Application.InspectionChecklists.GetInspectionChecklists;

public sealed record ChecklistItemResponse(
    Guid Id,
    string ItemName,
    string Category,
    bool IsRequired,
    int SortOrder);

public sealed record InspectionChecklistResponse(
    Guid Id,
    string Name,
    Guid? VehicleCategoryId,
    bool IsActive,
    List<ChecklistItemResponse> Items);

public sealed record GetInspectionChecklistsQuery : IQuery<List<InspectionChecklistResponse>>;
