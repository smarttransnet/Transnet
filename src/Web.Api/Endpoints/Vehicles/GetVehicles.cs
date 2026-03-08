using Application.Abstractions.Messaging;
using Application.Assets.GetVehicles;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Vehicles;

internal sealed class GetVehicles : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("vehicles", async (
            IQueryHandler<GetVehiclesQuery, List<VehicleResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetVehiclesQuery();

            Result<List<VehicleResponse>> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Vehicles);
    }
}
