using Application.Abstractions.Messaging;
using Application.InspectionCatalogItems.DeleteInspectionCatalogItem;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.InspectionCatalogItems;

internal sealed class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("inspection-catalog-items/{id:guid}", async (
            Guid id, 
            ICommandHandler<DeleteInspectionCatalogItemCommand> handler, 
            CancellationToken cancellationToken) =>
        {
            var command = new DeleteInspectionCatalogItemCommand(id);
            Result result = await handler.Handle(command, cancellationToken);
            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.InspectionCatalogItems);
    }
}
