using Application.Abstractions.Messaging;
using Application.Trips.TransitionTripStatus;
using Domain.Trips.Enums;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Trips;

public sealed class TransitionTripStatus : IEndpoint
{
    public record TransitionTripStatusRequest(
        TripStatus newStatus,
        string? notes,
        StatusChangeSource source,
        Guid? changedByUserId,
        Guid? changedByDriverId
    );

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("trips/{id:guid}/status", async (
            Guid id, 
            TransitionTripStatusRequest request,
            ICommandHandler<TransitionTripStatusCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new TransitionTripStatusCommand(
                id,
                request.newStatus,
                request.notes,
                request.source,
                request.changedByUserId,
                request.changedByDriverId,
                null,
                null);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.Trips);
    }
}

