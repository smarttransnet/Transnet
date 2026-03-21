using Application.Abstractions.Messaging;
using Application.Invoices.Commands.DeleteInvoice;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.ClientBilling.Invoices;

internal sealed class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("invoices/{id}", async (
            Guid id,
            ICommandHandler<DeleteInvoiceCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new DeleteInvoiceCommand(id);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.ClientBilling);
    }
}
