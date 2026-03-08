using Application.Abstractions.Messaging;
using Application.Drivers.Assignments.RejectAssignment;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.DriverAssignments;

internal sealed class Reject : IEndpoint
{
    public sealed record Request(string? RejectionReason);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("drivers/assignments/{id:guid}/reject", async (
            Guid id,
            Request request,
            ICommandHandler<RejectAssignmentCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new RejectAssignmentCommand(id, request.RejectionReason);
            Result result = await handler.Handle(command, cancellationToken);
            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.DriverAssignments);
    }
}
