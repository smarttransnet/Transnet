using Application.Abstractions.Messaging;
using Application.Trips.DeleteTripVoucher;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Trips;

public sealed class DeleteTripVoucher : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("trips/{id:guid}/voucher", async (
            Guid id, 
            ICommandHandler<DeleteTripVoucherCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new DeleteTripVoucherCommand(id);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.Trips);
    }
}
