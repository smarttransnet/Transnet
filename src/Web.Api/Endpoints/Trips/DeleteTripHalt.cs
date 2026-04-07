using Application.Abstractions.Messaging;
using Application.Trips.DeleteTripHalt;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Trips;

public sealed class DeleteTripHalt : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("trips/{id:guid}/halts/{haltId:guid}", async (
            Guid id, 
            Guid haltId, 
            ICommandHandler<DeleteTripHaltCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new DeleteTripHaltCommand(id, haltId);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.Trips);
    }
}
