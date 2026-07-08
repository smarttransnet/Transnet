using Application.Abstractions.Messaging;
using Application.Assets.UpdateVehicle;
using Domain.Assets.Enums;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Vehicles;

public sealed record UpdateVehicleRequest(
    string ChassisNumber,
    string PlateNumber,
    string Make,
    string Model,
    int Year,
    Guid VehicleCategoryId,
    VehicleType VehicleType,
    VehicleStatus Status,
    decimal OdometerReading,
    bool IsActive);

internal sealed class Update : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("vehicles/{id:guid}", async (
            Guid id,
            UpdateVehicleRequest request,
            ICommandHandler<UpdateVehicleCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new UpdateVehicleCommand(
                id,
                request.PlateNumber,
                request.ChassisNumber,
                request.Make,
                request.Model,
                request.Year,
                request.VehicleCategoryId,
                request.VehicleType,
                request.Status,
                request.OdometerReading,
                request.IsActive);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(() => Results.Ok(), CustomResults.Problem);
        })
        .WithTags(Tags.Vehicles);
    }
}
