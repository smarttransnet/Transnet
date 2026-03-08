using Application.Abstractions.Messaging;
using Application.InspectionChecklists.CreateInspectionChecklist;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.InspectionChecklists;

internal sealed class Create : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("inspection-checklists", async (
            CreateInspectionChecklistCommand request,
            ICommandHandler<CreateInspectionChecklistCommand, Guid> handler,
            CancellationToken cancellationToken) =>
        {
            Result<Guid> result = await handler.Handle(request, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.InspectionChecklists);
    }
}
