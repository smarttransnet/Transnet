using Application.Abstractions.Messaging;
using Application.Clients.Commands.RegisterClient;
using MediatR;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Clients;

internal sealed class Create : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("clients", async (
            RegisterClientCommand command,
            ICommandHandler<RegisterClientCommand, Guid> handler,
            CancellationToken cancellationToken) =>
        {
            Result<Guid> result = await handler.Handle(command, cancellationToken);
            return result.IsSuccess ? Results.Created($"/clients/{result.Value}", result.Value) : CustomResults.Problem(result);
        })
        .WithTags(Tags.Clients);
    }
}
