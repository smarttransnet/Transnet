using Application.InspectionCatalogItems.DeleteInspectionCatalogItem;
using Application.Abstractions.Messaging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Web.Api.Endpoints.InspectionCatalogItems;

public sealed class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("inspection-catalog-items/{id:guid}", async (Guid id, [FromServices] ICommandHandler<DeleteInspectionCatalogItemCommand> handler, CancellationToken cancellationToken) =>
        {
            var command = new DeleteInspectionCatalogItemCommand(id);
            var result = await handler.Handle(command, cancellationToken);
            return result.IsSuccess ? Results.NoContent() : Results.NotFound(result.Error);
        })
        .WithTags(Tags.InspectionCatalogItems);
    }
}
