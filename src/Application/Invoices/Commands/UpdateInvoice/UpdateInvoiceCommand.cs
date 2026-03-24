using Application.Abstractions.Messaging;
using Domain.Billing.Enums;

namespace Application.Invoices.Commands.UpdateInvoice;

public sealed record UpdateInvoiceLineItemCommand(
    Guid? Id,
    Guid? TripId,
    string Description,
    ServiceType ServiceType,
    decimal Quantity,
    decimal UnitPriceQAR,
    decimal DiscountPercent,
    decimal TaxPercent,
    int SortOrder);

public sealed record UpdateInvoiceCommand(
    Guid Id,
    DateOnly DueDate,
    InvoiceStatus Status,
    string? Notes,
    Guid? ReportFormatId,
    List<UpdateInvoiceLineItemCommand> LineItems) : ICommand;
