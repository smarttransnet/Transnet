using Application.Abstractions.Messaging;
using Domain.Billing.Enums;

namespace Application.Quotations.Commands.UpdateQuotation;

public sealed record UpdateQuotationLineItemCommand(
    Guid? Id,
    string Description,
    ServiceType ServiceType,
    decimal Quantity,
    decimal UnitPriceQAR,
    decimal DiscountPercent,
    decimal TaxPercent,
    int SortOrder);

public sealed record UpdateQuotationCommand(
    Guid Id,
    DateOnly ValidUntil,
    QuotationStatus Status,
    string? Notes,
    string? TermsAndConditions,
    List<UpdateQuotationLineItemCommand> LineItems) : ICommand;
