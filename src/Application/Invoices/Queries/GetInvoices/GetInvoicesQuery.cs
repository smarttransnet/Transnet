using Application.Abstractions.Messaging;
using Domain.Billing.Enums;

namespace Application.Invoices.Queries.GetInvoices;

public sealed record GetInvoicesQuery(
    Guid? ClientId = null,
    InvoiceStatus? Status = null,
    string? SearchTerm = null
) : IQuery<IReadOnlyList<InvoiceResponse>>;
