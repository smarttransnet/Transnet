using Application.Abstractions.Messaging;
using Application.Fuel.Woqood.Commands.UpsertWoqoodCardMapping;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.WoqoodCardMappings;

public sealed record UpsertCardMappingRequest(
    string WoqoodCardNumber,
    Guid? VehicleId,
    Guid? DriverId,
    string CardHolderName,
    string? Notes
);

internal sealed class UpsertCardMapping : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("fuel/woqood/card-mappings", async (
            UpsertCardMappingRequest request,
            ICommandHandler<UpsertWoqoodCardMappingCommand, Guid> handler,
            CancellationToken cancellationToken) =>
        {
            var mockUserId = Guid.NewGuid();

            var command = new UpsertWoqoodCardMappingCommand(
                request.WoqoodCardNumber,
                request.VehicleId,
                request.DriverId,
                request.CardHolderName,
                request.Notes,
                mockUserId
            );

            Result<Guid> result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.WoqoodCardMappings);
    }
}
