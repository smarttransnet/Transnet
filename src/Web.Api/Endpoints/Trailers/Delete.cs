using Application.Abstractions.Messaging;
using Application.Trailers.DeleteTrailer;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Trailers;

internal sealed class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("trailers/{id}", async (
            Guid id,
            ICommandHandler<DeleteTrailerCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new DeleteTrailerCommand(id);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.Trailers);
    }
}
