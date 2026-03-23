using Application.Abstractions.Messaging;
using Application.Trips.DeleteTrip;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Trips;

internal sealed class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("trips/{id}", async (
            Guid id,
            ICommandHandler<DeleteTripCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new DeleteTripCommand(id);
            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.Trips);
    }
}
