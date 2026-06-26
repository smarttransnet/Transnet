using Application.InspectionCatalogItems.CreateInspectionCatalogItem;
using Application.Abstractions.Messaging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Web.Api.Endpoints.InspectionCatalogItems;

public sealed class Create : IEndpoint
{
    public sealed record Request(
        string Category,
        string ItemName,
        int SortOrder,
        bool IsActive);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("inspection-catalog-items", async (Request request, [FromServices] ICommandHandler<CreateInspectionCatalogItemCommand, Guid> handler, CancellationToken cancellationToken) =>
        {
            var command = new CreateInspectionCatalogItemCommand(
                request.Category,
                request.ItemName,
                request.SortOrder,
                request.IsActive);
                
            var result = await handler.Handle(command, cancellationToken);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
        })
        .WithTags(Tags.InspectionCatalogItems);
    }
}
