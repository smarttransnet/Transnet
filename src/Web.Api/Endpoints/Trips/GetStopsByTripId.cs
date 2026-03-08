using Application.Abstractions.Messaging;
using Application.Trips.Common;
using Application.Trips.GetStopsByTripId;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Trips;

internal sealed class GetStopsByTripId : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("trips/{id:guid}/stops", async (
            Guid id,
            IQueryHandler<GetStopsByTripIdQuery, List<TripStopResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetStopsByTripIdQuery(id);

            Result<List<TripStopResponse>> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.TripStops);
    }
}
