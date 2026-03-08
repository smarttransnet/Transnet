using Application.Abstractions.Messaging;
using Application.Trips.CreateTripVoucher;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Trips;

public sealed class CreateTripVoucher : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("trips/{id:guid}/voucher", async (
            Guid id, 
            CreateTripVoucherRequest request, 
            ICommandHandler<CreateTripVoucherCommand, Guid> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new CreateTripVoucherCommand(
                id,
                request.VoucherNumber,
                request.VoucherDate,
                request.Notes,
                request.CreatedByUserId);

            Result<Guid> result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Trips);
    }
}

public sealed record CreateTripVoucherRequest(
    string VoucherNumber,
    DateTime VoucherDate,
    string? Notes,
    Guid CreatedByUserId);
