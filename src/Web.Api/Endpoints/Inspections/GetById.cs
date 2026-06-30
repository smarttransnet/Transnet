using Microsoft.AspNetCore.Mvc;
using Application.Abstractions.Messaging;
using Microsoft.AspNetCore.Http;
using Application.Inspections.GetInspectionById;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Inspections;

internal sealed class GetById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("inspections/{id}", async (
            Guid id,
            [FromServices] IQueryHandler<GetInspectionByIdQuery, InspectionDetailedResponse> handler,
            CancellationToken cancellationToken) =>
        {
            Result<InspectionDetailedResponse> result = await handler.Handle(new GetInspectionByIdQuery(id), cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Inspections);
    }
}
