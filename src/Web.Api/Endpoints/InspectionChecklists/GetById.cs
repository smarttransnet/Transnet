using Application.Abstractions.Messaging;
using Application.InspectionChecklists;
using Application.InspectionChecklists.GetInspectionChecklistById;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.InspectionChecklists;

internal sealed class GetById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("inspection-checklists/{id:guid}", async (
            Guid id,
            IQueryHandler<GetInspectionChecklistByIdQuery, InspectionChecklistResponse> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetInspectionChecklistByIdQuery(id);

            Result<InspectionChecklistResponse> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.InspectionChecklists);
    }
}
