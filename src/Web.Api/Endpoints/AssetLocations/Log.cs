using Application.Abstractions.Messaging;
using Application.AssetLocations.LogAssetLocation;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.AssetLocations;

internal sealed class Log : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("locations", async (
            LogAssetLocationCommand request,
            ICommandHandler<LogAssetLocationCommand, Guid> handler,
            CancellationToken cancellationToken) =>
        {
            Result<Guid> result = await handler.Handle(request, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.AssetLocations);
    }
}
