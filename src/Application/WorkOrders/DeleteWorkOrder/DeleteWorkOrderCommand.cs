using Application.Abstractions.Messaging;

namespace Application.WorkOrders.DeleteWorkOrder;

public sealed record DeleteWorkOrderCommand(Guid Id) : ICommand;
