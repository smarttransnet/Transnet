using Application.Abstractions.Messaging;
using Application.Trips.UpdateTripVoucher;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Trips;

public sealed class UpdateTripVoucher : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("trips/{id:guid}/voucher/{voucherId:guid}", async (
            Guid id, 
            Guid voucherId, 
            UpdateTripVoucherRequest request, 
            ICommandHandler<UpdateTripVoucherCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new UpdateTripVoucherCommand(
                voucherId,
                id,
                request.VoucherNumber,
                request.VoucherDate,
                request.Notes);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.Trips);
    }
}

public sealed record UpdateTripVoucherRequest(
    string VoucherNumber,
    DateTime VoucherDate,
    string? Notes);
