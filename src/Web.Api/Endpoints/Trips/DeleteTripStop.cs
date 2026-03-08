using Application.Abstractions.Messaging;
using Application.Trips.DeleteTripStop;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Trips;

public sealed class DeleteTripStop : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("trips/{id:guid}/stops/{stopId:guid}", async (
            Guid id, 
            Guid stopId, 
            ICommandHandler<DeleteTripStopCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new DeleteTripStopCommand(stopId, id);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.Trips);
    }
}
