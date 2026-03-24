using Application.Abstractions.Messaging;
using Application.AssetLocations.DeleteAssetLocation;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.AssetLocations;

internal sealed class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("asset-locations/{id}", async (
            Guid id,
            ICommandHandler<DeleteAssetLocationCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new DeleteAssetLocationCommand(id);
            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.AssetLocations);
    }
}
