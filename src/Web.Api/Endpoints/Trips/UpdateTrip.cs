using Application.Abstractions.Messaging;
using Application.Trips.UpdateTrip;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Trips;

public sealed class UpdateTrip : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("trips/{id:guid}", async (
            Guid id, 
            UpdateTripRequest request, 
            ICommandHandler<UpdateTripCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new UpdateTripCommand(
                id,
                request.DriverId,
                request.VehicleId,
                request.TrailerId,
                request.ScheduledStartAt,
                request.TotalDistanceKm,
                request.ClientId,
                request.Origin,
                request.Destination,
                request.VehicleCategoryUomId,
                request.Quantity);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.Trips);
    }
}

public sealed record UpdateTripRequest(
    Guid DriverId,
    Guid VehicleId,
    Guid? TrailerId,
    DateTime ScheduledStartAt,
    decimal? TotalDistanceKm,
    Guid? ClientId,
    string Origin,
    string Destination,
    Guid? VehicleCategoryUomId = null,
    decimal? Quantity = null);
