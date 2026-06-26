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
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("trips/{id:guid}/status", async (
            Guid id, 
            [FromForm] TripStatus newStatus,
            [FromForm] string? notes,
            [FromForm] StatusChangeSource source,
            [FromForm] Guid? changedByUserId,
            [FromForm] Guid? changedByDriverId,
            IFormFile? photo,
            ICommandHandler<TransitionTripStatusCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new TransitionTripStatusCommand(
                id,
                newStatus,
                notes,
                source,
                changedByUserId,
                changedByDriverId,
                photo?.OpenReadStream(),
                photo?.FileName);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .DisableAntiforgery()
        .WithTags(Tags.Trips);
    }
}

