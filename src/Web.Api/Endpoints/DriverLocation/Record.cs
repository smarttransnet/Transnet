using Application.Abstractions.Messaging;
using Application.Drivers.Location.RecordLocation;
using Domain.Drivers.Enums;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.DriverLocation;

internal sealed class Record : IEndpoint
{
    public sealed record Request(Guid? TripId, double Latitude, double Longitude, float? Accuracy, float? SpeedKmh, float? Bearing, LocationSource Source);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("drivers/{driverId:guid}/location", async (
            Guid driverId,
            Request request,
            ICommandHandler<RecordLocationCommand, Guid> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new RecordLocationCommand(
                driverId,
                request.TripId,
                request.Latitude,
                request.Longitude,
                request.Accuracy,
                request.SpeedKmh,
                request.Bearing,
                request.Source);

            Result<Guid> result = await handler.Handle(command, cancellationToken);
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.DriverLocation);
    }
}
