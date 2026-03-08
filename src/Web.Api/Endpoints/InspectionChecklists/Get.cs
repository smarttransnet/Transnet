using Application.Abstractions.Messaging;
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
            IQueryHandler<GetInspectionChecklistsQuery, List<InspectionChecklistResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetInspectionChecklistsQuery();

            Result<List<InspectionChecklistResponse>> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.InspectionChecklists);
    }
}
