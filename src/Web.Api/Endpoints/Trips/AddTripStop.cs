using Application.Abstractions.Messaging;
using Application.Trips.AddTripStop;
using Domain.Trips.Enums;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Trips;

public sealed class AddTripStop : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("trips/{id:guid}/stops", async (
            Guid id, 
            AddTripStopRequest request, 
            ICommandHandler<AddTripStopCommand, Guid> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new AddTripStopCommand(
                id,
                request.StopOrder,
                request.StopType,
                request.LocationName,
                request.Address,
                request.Latitude,
                request.Longitude,
                request.PocName,
                request.PocPhone,
                request.PocEmail,
                request.ScheduledArrivalAt,
                request.Notes);

            Result<Guid> result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Trips);
    }
}

public sealed record AddTripStopRequest(
    int StopOrder,
    StopType StopType,
    string LocationName,
    string? Address,
    double? Latitude,
    double? Longitude,
    string? PocName,
    string? PocPhone,
    string? PocEmail,
    DateTime? ScheduledArrivalAt,
    string? Notes);
