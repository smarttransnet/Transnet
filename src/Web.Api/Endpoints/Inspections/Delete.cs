using Application.Abstractions.Messaging;
using Application.Inspections.DeleteVehicleInspection;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Inspections;

internal sealed class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("vehicle-inspections/{id}", async (
            Guid id,
            ICommandHandler<DeleteVehicleInspectionCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new DeleteVehicleInspectionCommand(id);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.Inspections);
    }
}
