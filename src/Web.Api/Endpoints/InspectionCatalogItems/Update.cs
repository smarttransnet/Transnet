using Application.InspectionCatalogItems.UpdateInspectionCatalogItem;
using Application.Abstractions.Messaging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Web.Api.Endpoints.InspectionCatalogItems;

public sealed class Update : IEndpoint
{
    public sealed record Request(
        string Category,
        string ItemName,
        int SortOrder,
        bool IsActive);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("inspection-catalog-items/{id:guid}", async (Guid id, Request request, [FromServices] ICommandHandler<UpdateInspectionCatalogItemCommand> handler, CancellationToken cancellationToken) =>
        {
            var command = new UpdateInspectionCatalogItemCommand(
                id,
                request.Category,
                request.ItemName,
                request.SortOrder,
                request.IsActive);
                
            var result = await handler.Handle(command, cancellationToken);
            return result.IsSuccess ? Results.NoContent() : Results.NotFound(result.Error);
        })
        .WithTags(Tags.InspectionCatalogItems);
    }
}
