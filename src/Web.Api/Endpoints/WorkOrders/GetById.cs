using Application.Abstractions.Messaging;
using Application.WorkOrders.GetWorkOrderById;
using Application.WorkOrders.GetWorkOrders;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.WorkOrders;

internal sealed class GetById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("work-orders/{id:guid}", async (
            Guid id,
            IQueryHandler<GetWorkOrderByIdQuery, WorkOrderResponse> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetWorkOrderByIdQuery(id);

            Result<WorkOrderResponse> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.WorkOrders);
    }
}
