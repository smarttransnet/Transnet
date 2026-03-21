using Application.Abstractions.Messaging;
using Domain.WorkOrders.Enums;
using SharedKernel;

namespace Application.WorkOrders.UpdateWorkOrder;

public sealed record WorkOrderItemUpdateCommand(
    Guid? Id,
    WorkOrderItemType ItemType,
    string Description,
    decimal Quantity,
    decimal UnitCostQAR);

public sealed record UpdateWorkOrderCommand(
    Guid Id,
    string Title,
    string Description,
    WorkOrderPriority Priority,
    Guid? AssignedTechnicianId,
    decimal? EstimatedCostQAR,
    DateTime? ScheduledDate,
    List<WorkOrderItemUpdateCommand> Items) : ICommand;
