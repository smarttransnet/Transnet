using Application.Abstractions.Messaging;
using Application.InspectionChecklists.UpdateInspectionChecklist;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.InspectionChecklists;

internal sealed class Update : IEndpoint
{
    public sealed record Request(
        string Name,
        Domain.Inspections.Enums.InspectionType InspectionType,
        string ApplicableVehicleTypes,
        bool IsActive,
        List<ChecklistItemUpdateCommand> Items);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("inspection-checklists/{id:guid}", async (
            Guid id,
            Request request,
            ICommandHandler<UpdateInspectionChecklistCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new UpdateInspectionChecklistCommand(
                id,
                request.Name,
                request.InspectionType,
                request.ApplicableVehicleTypes,
                request.IsActive,
                request.Items);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.InspectionChecklists);
    }
}
