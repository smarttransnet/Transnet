using Application.Abstractions.Messaging;
using Application.Trailers.UpdateTrailer;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Trailers;

internal sealed class Update : IEndpoint
{
    public sealed record Request(
        string TrailerNumber,
        string TrailerType,
        decimal Capacity,
        string CapacityUnit,
        bool IsActive);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("trailers/{id:guid}", async (
            Guid id,
            Request request,
            ICommandHandler<UpdateTrailerCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new UpdateTrailerCommand(
                id,
                request.TrailerNumber,
                request.TrailerType,
                request.Capacity,
                request.CapacityUnit,
                request.IsActive);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.Trailers);
    }
}
