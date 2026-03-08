using Application.Abstractions.Messaging;
using Application.Trailers.AttachTrailer;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Trailers;

internal sealed class Attach : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("trailers/{id:guid}/attach/{vehicleId:guid}", async (
            Guid id,
            Guid vehicleId,
            ICommandHandler<AttachTrailerCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new AttachTrailerCommand(id, vehicleId);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(() => Results.Ok(), CustomResults.Problem);
        })
        .WithTags(Tags.Trailers);
    }
}
