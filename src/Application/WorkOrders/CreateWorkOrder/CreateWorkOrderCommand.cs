using Application.Abstractions.Messaging;
using Domain.WorkOrders.Enums;

namespace Application.WorkOrders.CreateWorkOrder;

public sealed record CreateWorkOrderItemCommand(
    WorkOrderItemType ItemType,
    string Description,
    decimal Quantity,
    decimal UnitCostQAR);

public sealed record CreateWorkOrderCommand(
    string Title,
    string Description,
    Guid VehicleId,
    Guid? VehicleInspectionId,
    WorkOrderPriority Priority,
    Guid? AssignedTechnicianId,
    decimal? EstimatedCostQAR,
    DateTime? ScheduledDate,
    List<CreateWorkOrderItemCommand> Items) : ICommand<Guid>;
