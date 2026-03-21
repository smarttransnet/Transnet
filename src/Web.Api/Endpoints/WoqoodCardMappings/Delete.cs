using Application.Abstractions.Messaging;
using Application.Fuel.Woqood.Commands.DeleteWoqoodCardMapping;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.WoqoodCardMappings;

internal sealed class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("woqood-card-mappings/{id}", async (
            Guid id,
            ICommandHandler<DeleteWoqoodCardMappingCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new DeleteWoqoodCardMappingCommand(id);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.WoqoodCardMappings);
    }
}
