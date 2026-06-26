using Application.Abstractions.Messaging;
using Application.InspectionCatalogItems.GetInspectionCatalogItems;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Web.Api.Endpoints.InspectionCatalogItems;

public sealed class Get : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("inspection-catalog-items", async ([FromServices] IQueryHandler<GetInspectionCatalogItemsQuery, List<InspectionCatalogItemResponse>> handler, CancellationToken cancellationToken) =>
        {
            var query = new GetInspectionCatalogItemsQuery();
            var result = await handler.Handle(query, cancellationToken);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound();
        })
        .WithTags(Tags.InspectionCatalogItems);
    }
}
