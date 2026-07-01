using Application.Abstractions.Messaging;
using Application.InspectionCatalogItems.CreateInspectionCatalogItem;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Web.Api.Endpoints.InspectionCatalogItems;

internal sealed class Create : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("inspection-catalog-items", async (
            CreateInspectionCatalogItemCommand request,
            ICommandHandler<CreateInspectionCatalogItemCommand, Guid> handler,
            CancellationToken cancellationToken) =>
        {
            Result<Guid> result = await handler.Handle(request, cancellationToken);
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.InspectionCatalogItems);
    }
}
