using Application.Abstractions.Messaging;
using Application.InspectionChecklists;
using Application.InspectionChecklists.GetInspectionChecklists;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.InspectionChecklists;

internal sealed class Get : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("inspection-checklists", async (
            IQueryHandler<GetInspectionChecklistsQuery, IReadOnlyList<InspectionChecklistResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetInspectionChecklistsQuery();

            var result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.InspectionChecklists);
    }
}
