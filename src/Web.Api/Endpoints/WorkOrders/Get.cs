using Application.Abstractions.Messaging;
using Application.WorkOrders.GetWorkOrders;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.WorkOrders;

internal sealed class Get : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("work-orders", async (
            IQueryHandler<GetWorkOrdersQuery, List<WorkOrderResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetWorkOrdersQuery();

            Result<List<WorkOrderResponse>> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.WorkOrders);
    }
}
