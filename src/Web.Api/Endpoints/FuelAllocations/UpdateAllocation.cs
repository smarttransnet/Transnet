using Application.Abstractions.Authentication;
using Application.Abstractions.Messaging;
using Application.Fuel.Allocations.Commands.UpdateFuelAllocation;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.FuelAllocations;

public sealed record UpdateAllocationRequest(
    Guid VehicleId,
    Guid? TripId,
    decimal QuantityLitres,
    decimal AmountQAR,
    DateOnly AllocationDate,
    string? Notes
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
            var command = new UpdateFuelAllocationCommand(
                id,
                request.VehicleId,
                request.TripId,
                request.QuantityLitres,
                request.AmountQAR,
                request.AllocationDate,
                request.Notes,
                userContext.UserId
            );

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.FuelAllocations);
    }
}
