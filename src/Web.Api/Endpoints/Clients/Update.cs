using Application.Abstractions.Messaging;
using Application.Clients.Commands.UpdateClient;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Clients;

internal sealed class Update : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("clients/{id}", async (
            Guid id,
            UpdateClientCommand command,
            ICommandHandler<UpdateClientCommand> handler,
            CancellationToken cancellationToken) =>
        {
            if (id != command.ClientId)
            {
                return Results.BadRequest("ID mismatch");
            }
            Result result = await handler.Handle(command, cancellationToken);

            return result.IsSuccess ? Results.NoContent() : CustomResults.Problem(result);
        })
        .WithTags(Tags.Clients);
    }
}
