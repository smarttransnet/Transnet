using Application.Abstractions.Messaging;
using Application.WorkOrders.DeleteWorkOrder;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.WorkOrders;

internal sealed class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("work-orders/{id}", async (
            Guid id,
            ICommandHandler<DeleteWorkOrderCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new DeleteWorkOrderCommand(id);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.WorkOrders);
    }
}
