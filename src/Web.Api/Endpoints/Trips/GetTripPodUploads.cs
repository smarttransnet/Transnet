using Application.Abstractions.Messaging;
using Application.Trips.GetTripPodUploads;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Trips;

public sealed class GetTripPodUploads : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("trips/{id:guid}/pod-uploads", async (
            Guid id, 
            IQueryHandler<GetTripPodUploadsQuery, List<Application.Trips.Common.TripPodUploadResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetTripPodUploadsQuery(id);

            Result<List<Application.Trips.Common.TripPodUploadResponse>> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Trips);
    }
}
