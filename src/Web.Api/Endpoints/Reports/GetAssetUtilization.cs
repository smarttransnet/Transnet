using Application.Abstractions.Messaging;
using Application.Reports.GetAssetUtilization;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Reports;

internal sealed class GetAssetUtilization : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("reports/asset-utilization", async (
            DateTime startDate,
            DateTime endDate,
            IQueryHandler<GetAssetUtilizationQuery, List<AssetUtilizationResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetAssetUtilizationQuery(startDate, endDate);

            Result<List<AssetUtilizationResponse>> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Reports);
    }
}
