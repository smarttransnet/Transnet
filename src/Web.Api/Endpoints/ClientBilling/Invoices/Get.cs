using Application.Abstractions.Messaging;
using Application.Invoices.Queries.GetInvoices;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.ClientBilling.Invoices;

internal sealed class Get : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("invoices", async (
            Guid? clientId,
            Domain.Billing.Enums.InvoiceStatus? status,
            string? searchTerm,
            IQueryHandler<GetInvoicesQuery, IReadOnlyList<InvoiceResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetInvoicesQuery(clientId, status, searchTerm);

            Result<IReadOnlyList<InvoiceResponse>> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.ClientBilling);
    }
}
