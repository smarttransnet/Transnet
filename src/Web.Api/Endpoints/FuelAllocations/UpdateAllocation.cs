using Application.Abstractions.Authentication;
using Application.Abstractions.Messaging;
using Application.Fuel.Allocations.Commands.UpdateFuelAllocation;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.FuelAllocations;

public sealed record UpdateAllocationRequest(
    Guid VehicleId,
    string? TripId,
    decimal Liters,
    decimal Amount,
    string FuelType,
    DateOnly Date,
    string? Remarks
);

internal sealed class UpdateAllocation : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("fuel/allocations/{id}", async (
            Guid id,
            UpdateAllocationRequest request,
            IUserContext userContext,
            ICommandHandler<UpdateFuelAllocationCommand> handler,
            CancellationToken cancellationToken) =>
        {
            Guid? tripId = null;
            if (!string.IsNullOrWhiteSpace(request.TripId) && Guid.TryParse(request.TripId, out var parsedGuid))
            {
                tripId = parsedGuid;
            }

            if (!Enum.TryParse<Domain.Fuel.Enums.FuelType>(request.FuelType, true, out var fuelType))
            {
                fuelType = Domain.Fuel.Enums.FuelType.Other;
            }

            var command = new UpdateFuelAllocationCommand(
                id,
                request.VehicleId,
                tripId,
                request.Liters,
                request.Amount,
                fuelType,
                request.Date,
                request.Remarks,
                userContext.UserId
            );

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.FuelAllocations);
    }
}
