using Application.Abstractions.Messaging;
using Application.Fuel.Allocations.Commands.CreateFuelAllocation;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.FuelAllocations;

public sealed record CreateAllocationRequest(
    Guid VehicleId,
    string? TripId,
    decimal Liters,
    decimal Amount,
    string FuelType,
    DateOnly Date,
    string? Remarks
);

internal sealed class CreateAllocation : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("fuel/allocations", async (
            CreateAllocationRequest request,
            ICommandHandler<CreateFuelAllocationCommand, Guid> handler,
            CancellationToken cancellationToken) =>
        {
            var mockUserId = Guid.NewGuid();

            Guid? tripId = null;
            if (!string.IsNullOrWhiteSpace(request.TripId) && Guid.TryParse(request.TripId, out var parsedGuid))
            {
                tripId = parsedGuid;
            }

            if (!Enum.TryParse<Domain.Fuel.Enums.FuelType>(request.FuelType, true, out var fuelType))
            {
                fuelType = Domain.Fuel.Enums.FuelType.Other;
            }

            var command = new CreateFuelAllocationCommand(
                request.VehicleId,
                tripId,
                request.Liters,
                request.Amount,
                fuelType,
                request.Date,
                request.Remarks,
                mockUserId
            );

            Result<Guid> result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.FuelAllocations);
    }
}
