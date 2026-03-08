using Application.Abstractions.Messaging;

namespace Application.WorkOrders.GetWorkOrderById;

public sealed record GetWorkOrderByIdQuery(Guid WorkOrderId) : IQuery<GetWorkOrders.WorkOrderResponse>;
