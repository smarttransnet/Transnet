using Application.Abstractions.Messaging;
using Application.Assets.GetVehicles;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Vehicles;

internal sealed class GetVehicleById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("vehicles/{id:guid}", async (
            Guid id,
            IQueryHandler<GetVehicleByIdQuery, VehicleResponse> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetVehicleByIdQuery(id);

            Result<VehicleResponse> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Vehicles);
    }
}
