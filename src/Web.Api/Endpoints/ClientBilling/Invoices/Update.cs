using Application.Abstractions.Messaging;
using Application.Invoices.Commands.UpdateInvoice;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.ClientBilling.Invoices;

internal sealed class Update : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("invoices/{id:guid}", async (
            Guid id,
            UpdateInvoiceCommand request,
            ICommandHandler<UpdateInvoiceCommand> handler,
            CancellationToken cancellationToken) =>
        {
            if (id != request.Id)
            {
                return Results.BadRequest("Mismatched ID");
            }

            Result result = await handler.Handle(request, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.ClientBilling);
    }
}
