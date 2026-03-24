using Application.Abstractions.Messaging;
using Application.Drivers.DeleteDriver;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Drivers;

internal sealed class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("drivers/{id}", async (
            Guid id,
            ICommandHandler<DeleteDriverCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new DeleteDriverCommand(id);
            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.Drivers);
    }
}
