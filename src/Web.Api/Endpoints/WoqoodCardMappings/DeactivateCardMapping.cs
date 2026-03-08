using Application.Abstractions.Messaging;
using Application.Fuel.Woqood.Commands.DeactivateWoqoodCardMapping;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.WoqoodCardMappings;

internal sealed class DeactivateCardMapping : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("fuel/woqood/card-mappings/{id:guid}", async (
            Guid id,
            ICommandHandler<DeactivateWoqoodCardMappingCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new DeactivateWoqoodCardMappingCommand(id);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(() => Results.NoContent(), CustomResults.Problem);
        })
        .WithTags(Tags.WoqoodCardMappings);
    }
}
