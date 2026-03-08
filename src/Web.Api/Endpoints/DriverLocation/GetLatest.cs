using Application.Abstractions.Messaging;
using Application.Drivers.Location.GetLatestLocation;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.DriverLocation;

internal sealed class GetLatest : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("drivers/{driverId:guid}/location/latest", async (
            Guid driverId,
            IQueryHandler<GetLatestLocationQuery, LocationResponse> handler,
            CancellationToken cancellationToken) =>
        {
            Result<LocationResponse> result = await handler.Handle(new GetLatestLocationQuery(driverId), cancellationToken);
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.DriverLocation);
    }
}
