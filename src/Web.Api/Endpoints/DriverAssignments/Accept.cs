using Application.Abstractions.Messaging;
using Application.Drivers.Assignments.AcceptAssignment;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.DriverAssignments;

internal sealed class Accept : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("drivers/assignments/{id:guid}/accept", async (
            Guid id,
            ICommandHandler<AcceptAssignmentCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new AcceptAssignmentCommand(id);
            Result result = await handler.Handle(command, cancellationToken);
            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.DriverAssignments);
    }
}
