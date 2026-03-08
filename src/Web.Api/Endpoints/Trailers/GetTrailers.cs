using Application.Abstractions.Messaging;
using Application.Trailers.GetTrailers;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Trailers;

internal sealed class GetTrailers : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("trailers", async (
            IQueryHandler<GetTrailersQuery, List<TrailerResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetTrailersQuery();

            Result<List<TrailerResponse>> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Trailers);
    }
}
