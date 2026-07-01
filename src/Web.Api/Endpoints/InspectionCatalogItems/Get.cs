using Application.Abstractions.Messaging;
using Application.InspectionCatalogItems.GetInspectionCatalogItems;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.InspectionCatalogItems;

internal sealed class Get : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("inspection-catalog-items", async (
            IQueryHandler<GetInspectionCatalogItemsQuery, List<InspectionCatalogItemResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetInspectionCatalogItemsQuery();
            Result<List<InspectionCatalogItemResponse>> result = await handler.Handle(query, cancellationToken);
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.InspectionCatalogItems);
    }
}
