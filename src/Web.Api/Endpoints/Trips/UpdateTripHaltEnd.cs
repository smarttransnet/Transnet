using Application.Abstractions.Messaging;
using Application.Trips.UpdateTripHaltEnd;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Trips;

public sealed class UpdateTripHaltEnd : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("trips/{id:guid}/halts/{haltId:guid}/end", async (
            Guid id, 
            Guid haltId, 
            UpdateTripHaltEndRequest request, 
            ICommandHandler<UpdateTripHaltEndCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new UpdateTripHaltEndCommand(haltId, id, request.EndedAt);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.Trips);
    }
}

public sealed record UpdateTripHaltEndRequest(DateTime EndedAt);
