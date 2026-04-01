using Application.Abstractions.Messaging;
using Domain.Inspections.Enums;

namespace Application.InspectionChecklists.CreateInspectionChecklist;

public sealed record ChecklistItemCommand(
    string ItemName,
    string Category,
    bool IsRequired,
    int SortOrder);

public sealed record CreateInspectionChecklistCommand(
    string Name,
    Guid? VehicleCategoryId,
    InspectionType InspectionType,
    string ApplicableVehicleTypes,
    List<ChecklistItemCommand> Items) : ICommand<Guid>;
