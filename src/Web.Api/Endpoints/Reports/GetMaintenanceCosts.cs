using Application.Abstractions.Messaging;
using Application.Reports.GetMaintenanceCosts;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Reports;

internal sealed class GetMaintenanceCosts : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("reports/maintenance-costs", async (
            DateTime startDate,
            DateTime endDate,
            IQueryHandler<GetMaintenanceCostsQuery, List<MaintenanceCostResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetMaintenanceCostsQuery(startDate, endDate);

            Result<List<MaintenanceCostResponse>> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Reports);
    }
}
