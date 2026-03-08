using Application.Abstractions.Messaging;
using Application.Trips.GetHaltsByTripId;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Trips;

public sealed class GetTripHalts : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("trips/{id:guid}/halts", async (
            Guid id, 
            IQueryHandler<GetHaltsByTripIdQuery, List<Application.Trips.Common.TripHaltResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetHaltsByTripIdQuery(id);

            Result<List<Application.Trips.Common.TripHaltResponse>> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Trips);
    }
}
