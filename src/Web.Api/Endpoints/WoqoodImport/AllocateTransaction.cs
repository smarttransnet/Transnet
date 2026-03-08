using Application.Abstractions.Messaging;
using Application.Fuel.Woqood.Commands.AllocateWoqoodTransaction;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.WoqoodImport;

public sealed record AllocateTransactionRequest(
    Guid VehicleId,
    Guid? TripId,
    string? Notes
);

internal sealed class AllocateTransaction : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("fuel/woqood/transactions/{id:guid}/allocate", async (
            Guid id,
            AllocateTransactionRequest request,
            ICommandHandler<AllocateWoqoodTransactionCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var mockUserId = Guid.NewGuid(); // To be retrieved from UserContext/Claims
            var command = new AllocateWoqoodTransactionCommand(
                id,
                request.VehicleId,
                request.TripId,
                mockUserId,
                request.Notes
            );

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(() => Results.Ok(), CustomResults.Problem);
        })
        .WithTags(Tags.WoqoodImport);
    }
}
