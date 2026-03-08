using Application.Abstractions.Messaging;
using Application.AssetLocations.GetAssetLocationHistory;
using Domain.WorkOrders.Enums;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.AssetLocations;

internal sealed class GetHistory : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("locations/{assetType}/{assetId:guid}", async (
            AssetType assetType,
            Guid assetId,
            IQueryHandler<GetAssetLocationHistoryQuery, List<AssetLocationResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetAssetLocationHistoryQuery(assetType, assetId);

            Result<List<AssetLocationResponse>> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.AssetLocations);
    }
}
