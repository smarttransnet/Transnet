using Application.Abstractions.Messaging;
using Application.Clients.Commands.UpdateClient;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Clients;

internal sealed class Update : IEndpoint
{
    public sealed record Request(
        string CompanyName,
        string ContactPersonName,
        string ContactEmail,
        string ContactPhone,
        string BillingAddress,
        string? TaxRegistrationNumber,
        int PaymentTermsDays,
        string CurrencyCode,
        bool IsActive);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("clients/{id:guid}", async (
            Guid id,
            Request request,
            ICommandHandler<UpdateClientCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new UpdateClientCommand(
                id,
                request.CompanyName,
                request.ContactPersonName,
                request.ContactEmail,
                request.ContactPhone,
                request.BillingAddress,
                request.TaxRegistrationNumber,
                request.PaymentTermsDays,
                request.CurrencyCode,
                request.IsActive);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.Clients);
    }
}
