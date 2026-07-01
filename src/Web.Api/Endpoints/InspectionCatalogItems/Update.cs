using Application.Abstractions.Messaging;
using Application.InspectionCatalogItems.UpdateInspectionCatalogItem;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.InspectionCatalogItems;

internal sealed class Update : IEndpoint
{
    public sealed record Request(
        string Category,
        string ItemName,
        int SortOrder,
        bool IsActive);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("inspection-catalog-items/{id:guid}", async (
            Guid id, 
            Request request, 
            ICommandHandler<UpdateInspectionCatalogItemCommand> handler, 
            CancellationToken cancellationToken) =>
        {
            var command = new UpdateInspectionCatalogItemCommand(
                id,
                request.Category,
                request.ItemName,
                request.SortOrder,
                request.IsActive);
                
            Result result = await handler.Handle(command, cancellationToken);
            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.InspectionCatalogItems);
    }
}
