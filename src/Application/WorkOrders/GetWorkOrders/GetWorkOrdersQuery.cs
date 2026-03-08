using Application.Abstractions.Messaging;

namespace Application.WorkOrders.GetWorkOrders;

public sealed record GetWorkOrdersQuery : IQuery<List<WorkOrderResponse>>;
