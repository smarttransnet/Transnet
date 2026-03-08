using Application.Abstractions.Messaging;
using Domain.Billing.Enums;

namespace Application.Invoices.Commands.CreateInvoice;

public sealed record CreateInvoiceCommand(
    Guid ClientId,
    Guid IssuedByUserId,
    DateOnly DueDate,
    string? Notes,
    List<InvoiceLineItemRequest> LineItems
) : ICommand<Guid>;

public sealed record InvoiceLineItemRequest(
    string Description,
    ServiceType ServiceType,
    decimal Quantity,
    decimal UnitPriceQAR,
    decimal DiscountPercent,
    decimal TaxPercent,
    int SortOrder,
    Guid? TripId
);
