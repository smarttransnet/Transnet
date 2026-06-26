using Application.Abstractions.Messaging;
using Domain.Inspections.Enums;

namespace Application.InspectionChecklists.UpdateInspectionChecklist;

public sealed record UpdateChecklistItemCommand(
    Guid? Id,
    string ItemName,
    string Category,
    bool IsRequired,
    int SortOrder);

public sealed record UpdateInspectionChecklistCommand(
    Guid Id,
    string Name,
    Guid? VehicleCategoryId,
    bool IsActive,
    List<UpdateChecklistItemCommand> Items) : ICommand;
