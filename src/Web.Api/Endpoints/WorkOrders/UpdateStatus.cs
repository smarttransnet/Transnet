using Application.Abstractions.Messaging;
using Application.WorkOrders.UpdateWorkOrderStatus;
using Domain.WorkOrders.Enums;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.WorkOrders;

public sealed record UpdateWorkOrderStatusRequest(
    WorkOrderStatus Status,
    string? Notes,
    Guid ChangedByUserId);

internal sealed class UpdateStatus : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("work-orders/{id:guid}/status", async (
            Guid id,
            UpdateWorkOrderStatusRequest request,
            ICommandHandler<UpdateWorkOrderStatusCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new UpdateWorkOrderStatusCommand(
                id,
                request.Status,
                request.Notes,
                request.ChangedByUserId);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(() => Results.Ok(), CustomResults.Problem);
        })
        .WithTags(Tags.WorkOrders);
    }
}
