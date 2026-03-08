using Application.Abstractions.Messaging;
using Application.Fuel.Summaries.Queries.GetVehicleFuelSummaries;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.FuelSummary;

internal sealed class GetVehicleSummaries : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("fuel/vehicle-summary", async (
            Guid? vehicleId,
            int? periodMonth,
            int? periodYear,
            IQueryHandler<GetVehicleFuelSummariesQuery, IReadOnlyList<VehicleFuelSummaryResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetVehicleFuelSummariesQuery(vehicleId, periodMonth, periodYear);

            Result<IReadOnlyList<VehicleFuelSummaryResponse>> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.FuelSummary);

        app.MapGet("fuel/vehicle-summary/{vehicleId:guid}", async (
            Guid vehicleId,
            IQueryHandler<GetVehicleFuelSummariesQuery, IReadOnlyList<VehicleFuelSummaryResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetVehicleFuelSummariesQuery(VehicleId: vehicleId);

            Result<IReadOnlyList<VehicleFuelSummaryResponse>> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.FuelSummary);
    }
}
