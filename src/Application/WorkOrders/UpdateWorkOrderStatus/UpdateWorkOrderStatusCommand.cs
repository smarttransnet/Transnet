using Application.Abstractions.Messaging;
using Domain.WorkOrders.Enums;

namespace Application.WorkOrders.UpdateWorkOrderStatus;

public sealed record UpdateWorkOrderStatusCommand(
    Guid WorkOrderId,
    WorkOrderStatus Status,
    string? Notes,
    Guid ChangedByUserId) : ICommand;
