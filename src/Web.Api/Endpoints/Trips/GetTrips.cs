using Application.Abstractions.Messaging;
using Application.Trips.Common;
using Application.Trips.GetTrips;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Trips;

internal sealed class GetTrips : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("trips", async (
            IQueryHandler<GetTripsQuery, List<TripResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetTripsQuery();

            Result<List<TripResponse>> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Trips);
    }
}
