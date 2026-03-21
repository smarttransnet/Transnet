using Application.Abstractions.Messaging;
using Application.Quotations.Commands.CreateQuotation;
using Domain.Billing.Enums;
using SharedKernel;

namespace Application.Quotations.Commands.UpdateQuotation;

public sealed record QuotationLineItemUpdateCommand(
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
    List<QuotationLineItemUpdateCommand> Items) : ICommand;
