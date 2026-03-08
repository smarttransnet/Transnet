using Application.Abstractions.Messaging;
using Application.Trips.RecordTripHalt;
using Domain.Trips.Enums;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Trips;

public sealed class RecordTripHalt : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("trips/{id:guid}/halts", async (
            Guid id, 
            RecordTripHaltRequest request, 
            ICommandHandler<RecordTripHaltCommand, Guid> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new RecordTripHaltCommand(
                id,
                request.HaltType,
                request.Reason,
                request.Latitude,
                request.Longitude,
                request.LocationName,
                request.StartedAt,
                request.RecordedByDriverId);

            Result<Guid> result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Trips);
    }
}

public sealed record RecordTripHaltRequest(
    HaltType HaltType,
    string? Reason,
    double? Latitude,
    double? Longitude,
    string? LocationName,
    DateTime StartedAt,
    Guid RecordedByDriverId);
