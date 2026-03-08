using Application.Abstractions.Messaging;
using Application.Trailers.GetTrailerPerformance;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Trailers;

internal sealed class GetPerformance : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("trailers/{id:guid}/performance", async (
            Guid id,
            IQueryHandler<GetTrailerPerformanceQuery, TrailerPerformanceResponse> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetTrailerPerformanceQuery(id);

            Result<TrailerPerformanceResponse> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Trailers);
    }
}
