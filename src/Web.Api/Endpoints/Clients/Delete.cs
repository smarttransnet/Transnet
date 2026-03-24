using Application.Abstractions.Messaging;
using Application.Clients.Commands.DeleteClient;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Clients;

internal sealed class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("clients/{id}", async (
            Guid id,
            ICommandHandler<DeleteClientCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new DeleteClientCommand(id);
            Result result = await handler.Handle(command, cancellationToken);

            return result.IsSuccess ? Results.NoContent() : CustomResults.Problem(result);
        })
        .WithTags(Tags.Clients);
    }
}
