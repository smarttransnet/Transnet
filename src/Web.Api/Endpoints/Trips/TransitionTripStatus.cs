using Application.Abstractions.Messaging;
using Application.Trips.TransitionTripStatus;
using Domain.Trips.Enums;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
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
            TransitionTripStatusRequest request, 
            ICommandHandler<TransitionTripStatusCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new TransitionTripStatusCommand(
                id,
                request.NewStatus,
                request.Notes,
                request.Source,
                request.ChangedByUserId,
                request.ChangedByDriverId);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.Trips);
    }
}

public sealed record TransitionTripStatusRequest(
    TripStatus NewStatus,
    string? Notes,
    StatusChangeSource Source,
    Guid? ChangedByUserId,
    Guid? ChangedByDriverId);
