using Application.Abstractions.Messaging;
using Application.InspectionChecklists.DeleteInspectionChecklist;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.InspectionChecklists;

internal sealed class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("inspection-checklists/{id}", async (
            Guid id,
            ICommandHandler<DeleteInspectionChecklistCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new DeleteInspectionChecklistCommand(id);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.InspectionChecklists);
    }
}
