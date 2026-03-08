using Application.Abstractions.Messaging;
using Application.Fuel.Allocations.Queries.GetFuelAllocations;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.FuelAllocations;

internal sealed class GetAllocations : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("fuel/allocations", async (
            Guid? vehicleId,
            Guid? tripId,
            DateOnly? startDate,
            DateOnly? endDate,
            IQueryHandler<GetFuelAllocationsQuery, IReadOnlyList<FuelAllocationResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetFuelAllocationsQuery(vehicleId, tripId, startDate, endDate);

            Result<IReadOnlyList<FuelAllocationResponse>> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.FuelAllocations);

        app.MapGet("fuel/allocations/vehicle/{vehicleId:guid}", async (
            Guid vehicleId,
            IQueryHandler<GetFuelAllocationsQuery, IReadOnlyList<FuelAllocationResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetFuelAllocationsQuery(VehicleId: vehicleId);

            Result<IReadOnlyList<FuelAllocationResponse>> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.FuelAllocations);

        app.MapGet("fuel/allocations/trip/{tripId:guid}", async (
            Guid tripId,
            IQueryHandler<GetFuelAllocationsQuery, IReadOnlyList<FuelAllocationResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetFuelAllocationsQuery(TripId: tripId);

            Result<IReadOnlyList<FuelAllocationResponse>> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.FuelAllocations);
    }
}
