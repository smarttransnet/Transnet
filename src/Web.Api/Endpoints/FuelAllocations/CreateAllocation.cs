using Application.Abstractions.Messaging;
using Application.Fuel.Allocations.Commands.CreateFuelAllocation;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.FuelAllocations;

public sealed record CreateAllocationRequest(
    Guid VehicleId,
    Guid? TripId,
    decimal QuantityLitres,
    decimal AmountQAR,
    DateOnly AllocationDate,
    string? Notes
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

            var command = new CreateFuelAllocationCommand(
                request.VehicleId,
                request.TripId,
                request.QuantityLitres,
                request.AmountQAR,
                request.AllocationDate,
                request.Notes,
                mockUserId
            );

            Result<Guid> result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.FuelAllocations);
    }
}
