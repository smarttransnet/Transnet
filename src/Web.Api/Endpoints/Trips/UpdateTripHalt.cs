using Application.Abstractions.Messaging;
using Application.Trips.UpdateTripHalt;
using Domain.Trips.Enums;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Trips;

public sealed class UpdateTripHalt : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("trips/{id:guid}/halts/{haltId:guid}", async (
            Guid id,
            Guid haltId,
            UpdateTripHaltRequest request,
            ICommandHandler<UpdateTripHaltCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new UpdateTripHaltCommand(
                id,
                haltId,
                (HaltType)request.HaltType,
                request.Reason,
                request.Latitude,
                request.Longitude,
                request.LocationName,
                request.StartedAt,
                request.EndedAt);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.Trips);
    }
}

public sealed record UpdateTripHaltRequest(
    int HaltType,
    string? Reason,
    double? Latitude,
    double? Longitude,
    string? LocationName,
    DateTime StartedAt,
    DateTime? EndedAt);
