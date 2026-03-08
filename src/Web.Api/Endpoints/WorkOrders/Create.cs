using Application.Abstractions.Messaging;
using Application.WorkOrders.CreateWorkOrder;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.WorkOrders;

internal sealed class Create : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("work-orders", async (
            CreateWorkOrderCommand request,
            ICommandHandler<CreateWorkOrderCommand, Guid> handler,
            CancellationToken cancellationToken) =>
        {
            Result<Guid> result = await handler.Handle(request, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.WorkOrders);
    }
}
