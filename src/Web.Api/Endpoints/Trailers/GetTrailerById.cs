using Application.Abstractions.Messaging;
using Application.Trailers.GetTrailerById;
using Application.Trailers.GetTrailers;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Trailers;

internal sealed class GetTrailerById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("trailers/{id:guid}", async (
            Guid id,
            IQueryHandler<GetTrailerByIdQuery, TrailerResponse> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetTrailerByIdQuery(id);

            Result<TrailerResponse> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Trailers);
    }
}
