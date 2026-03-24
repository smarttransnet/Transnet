using Application.Abstractions.Messaging;
using Application.InspectionChecklists.UpdateInspectionChecklist;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.InspectionChecklists;

internal sealed class Update : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("inspection-checklists/{id}", async (
            Guid id,
            UpdateInspectionChecklistCommand request,
            ICommandHandler<UpdateInspectionChecklistCommand> handler,
            CancellationToken cancellationToken) =>
        {
            if (id != request.Id)
            {
                return Results.BadRequest("ID mismatch");
            }
            Result result = await handler.Handle(request, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.InspectionChecklists);
    }
}
