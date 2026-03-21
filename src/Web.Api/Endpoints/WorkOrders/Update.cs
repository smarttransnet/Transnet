using Application.Abstractions.Messaging;
using Application.WorkOrders.UpdateWorkOrder;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.WorkOrders;

internal sealed class Update : IEndpoint
{
    public sealed record Request(
        string Title,
        string Description,
        Domain.WorkOrders.Enums.WorkOrderPriority Priority,
        Guid? AssignedTechnicianId,
        decimal? EstimatedCostQAR,
        DateTime? ScheduledDate,
        List<WorkOrderItemUpdateCommand> Items);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("work-orders/{id:guid}", async (
            Guid id,
            Request request,
            ICommandHandler<UpdateWorkOrderCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new UpdateWorkOrderCommand(
                id,
                request.Title,
                request.Description,
                request.Priority,
                request.AssignedTechnicianId,
                request.EstimatedCostQAR,
                request.ScheduledDate,
                request.Items);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.WorkOrders);
    }
}
