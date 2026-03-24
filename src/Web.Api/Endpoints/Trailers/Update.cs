using Application.Abstractions.Messaging;
using Application.Trailers.UpdateTrailer;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Trailers;

internal sealed class Update : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("trailers/{id}", async (
            Guid id,
            UpdateTrailerRequest request,
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

public sealed record UpdateTrailerRequest(
    string TrailerNumber,
    string TrailerType,
    decimal Capacity,
    string CapacityUnit,
    bool IsActive);
