using Application.Abstractions.Messaging;
using Application.Invoices.Queries.GetInvoiceById;
using Application.Invoices.Queries.GetInvoices;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.ClientBilling.Invoices;

internal sealed class GetById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("invoices/{id:guid}", async (
            Guid id,
            IQueryHandler<GetInvoiceByIdQuery, InvoiceDetailResponse> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetInvoiceByIdQuery(id);

            Result<InvoiceDetailResponse> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.ClientBilling);
    }
}
