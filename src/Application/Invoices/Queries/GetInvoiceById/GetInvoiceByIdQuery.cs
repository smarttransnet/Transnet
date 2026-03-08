using Application.Abstractions.Messaging;

namespace Application.Invoices.Queries.GetInvoiceById;

public sealed record GetInvoiceByIdQuery(Guid InvoiceId) : IQuery<InvoiceDetailResponse>;
