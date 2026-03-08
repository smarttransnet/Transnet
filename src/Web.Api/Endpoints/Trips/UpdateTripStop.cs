using Application.Abstractions.Messaging;
using Application.Trips.UpdateTripStop;
using Domain.Trips.Enums;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Trips;

public sealed class UpdateTripStop : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("trips/{id:guid}/stops/{stopId:guid}", async (
            Guid id, 
            Guid stopId, 
            UpdateTripStopRequest request, 
            ICommandHandler<UpdateTripStopCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new UpdateTripStopCommand(
                stopId,
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
                request.ActualArrivalAt,
                request.ActualDepartureAt,
                request.Notes);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.Trips);
    }
}

public sealed record UpdateTripStopRequest(
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
    DateTime? ActualArrivalAt,
    DateTime? ActualDepartureAt,
    string? Notes);
