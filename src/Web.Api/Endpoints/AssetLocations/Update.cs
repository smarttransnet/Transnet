using Application.Abstractions.Messaging;
using Application.AssetLocations.UpdateAssetLocation;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.AssetLocations;

internal sealed class Update : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("asset-locations/{id}", async (
            Guid id,
            UpdateAssetLocationRequest request,
            ICommandHandler<UpdateAssetLocationCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new UpdateAssetLocationCommand(id, request.Latitude, request.Longitude, request.LocationName);
            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.AssetLocations);
    }
}

public sealed record UpdateAssetLocationRequest(double Latitude, double Longitude, string? LocationName);
