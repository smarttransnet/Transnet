using Application.Abstractions.Messaging;
using Domain.WorkOrders.Enums;

namespace Application.WorkOrders.UpdateWorkOrder;

public sealed record UpdateWorkOrderCommand(
    Guid Id,
    string Title,
    string Description,
    WorkOrderPriority Priority,
    Guid? AssignedTechnicianId,
    decimal? EstimatedCostQAR,
    DateTime? ScheduledDate) : ICommand;
