namespace Application.Invoices.Queries.GetInvoices;

public sealed record InvoiceResponse(
    Guid Id,
    string InvoiceNumber,
    Guid ClientId,
    string ClientName,
    DateTime IssuedAt,
    DateOnly DueDate,
    string Status,
    decimal TotalQAR,
    decimal PaidAmountQAR,
    decimal OutstandingAmountQAR
);
