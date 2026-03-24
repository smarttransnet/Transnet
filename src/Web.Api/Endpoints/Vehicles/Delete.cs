using Application.Abstractions.Messaging;
using Application.Vehicles.DeleteVehicle;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Vehicles;

internal sealed class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("vehicles/{id}", async (
            Guid id,
            ICommandHandler<DeleteVehicleCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new DeleteVehicleCommand(id);
            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.Vehicles);
    }
}
