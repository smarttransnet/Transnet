using Application.Abstractions.Authentication;
using Application.Abstractions.Messaging;
using Application.Fuel.Woqood.Commands.UpsertWoqoodCardMapping;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.WoqoodCardMappings;

public sealed record UpsertCardMappingRequest(
    string CardNumber,
    Guid? VehicleId,
    Guid? DriverId,
    string CardHolderName,
    string? Notes,
    bool IsActive
);

internal sealed class UpsertCardMapping : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("fuel/woqood/card-mappings", async (
            UpsertCardMappingRequest request,
            IUserContext userContext,
            ICommandHandler<UpsertWoqoodCardMappingCommand, Guid> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new UpsertWoqoodCardMappingCommand(
                request.CardNumber,
                request.VehicleId,
                request.DriverId,
                request.CardHolderName,
                request.Notes,
                request.IsActive,
                userContext.UserId
            );

            Result<Guid> result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.WoqoodCardMappings);
    }
}
