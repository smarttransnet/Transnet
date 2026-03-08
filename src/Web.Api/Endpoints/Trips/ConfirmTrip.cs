using Application.Abstractions.Messaging;
using Application.Trips.ConfirmTrip;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Trips;

public sealed class ConfirmTrip : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("trips/{id:guid}/confirm", async (
            Guid id, 
            ConfirmTripRequest request, 
            ICommandHandler<ConfirmTripCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new ConfirmTripCommand(id, request.DriverId);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.Trips);
    }
}

public sealed record ConfirmTripRequest(Guid DriverId);
