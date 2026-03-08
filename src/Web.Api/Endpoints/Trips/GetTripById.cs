using Application.Abstractions.Messaging;
using Application.Trips.GetTripById;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Trips;

public sealed class GetTripById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("trips/{id:guid}", async (
            Guid id, 
            IQueryHandler<GetTripByIdQuery, Application.Trips.Common.TripResponse> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetTripByIdQuery(id);

            Result<Application.Trips.Common.TripResponse> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Trips);
    }
}
