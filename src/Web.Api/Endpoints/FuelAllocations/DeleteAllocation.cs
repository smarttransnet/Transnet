using Application.Abstractions.Messaging;
using Application.Fuel.Allocations.Commands.DeleteFuelAllocation;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.FuelAllocations;

internal sealed class DeleteAllocation : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("fuel/allocations/{id}", async (
            Guid id,
            ICommandHandler<DeleteFuelAllocationCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new DeleteFuelAllocationCommand(id);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.FuelAllocations);
    }
}
