using Application.Abstractions.Messaging;
using Domain.Billing.Enums;
using SharedKernel;

namespace Application.Invoices.Commands.UpdateInvoice;

public sealed record InvoiceLineItemUpdateCommand(
    Guid? Id,
    string Description,
    ServiceType ServiceType,
    decimal Quantity,
    decimal UnitPriceQAR,
    decimal DiscountPercent,
    decimal TaxPercent,
    int SortOrder,
    Guid? TripId);

public sealed record UpdateInvoiceCommand(
    Guid Id,
    DateOnly DueDate,
    InvoiceStatus Status,
    string? Notes,
    List<InvoiceLineItemUpdateCommand> Items) : ICommand;
