using Application.Abstractions.Messaging;
using Domain.Billing.Enums;

namespace Application.Quotations.Commands.CreateQuotation;

public sealed record CreateQuotationCommand(
    Guid ClientId,
    Guid IssuedByUserId,
    DateOnly ValidUntil,
    string? Notes,
    string? TermsAndConditions,
    List<QuotationLineItemRequest> LineItems
) : ICommand<Guid>;

public sealed record QuotationLineItemRequest(
    string Description,
    ServiceType ServiceType,
    decimal Quantity,
    decimal UnitPriceQAR,
    decimal DiscountPercent,
    decimal TaxPercent,
    int SortOrder
);
