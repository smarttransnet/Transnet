namespace Application.Quotations.Queries.GetQuotationById;

public sealed record QuotationDetailResponse(
    // Same properties as above + LineItems
    Guid Id,
    string QuotationNumber,
    Guid ClientId,
    string ClientName,
    DateTime IssuedAt,
    DateOnly ValidUntil,
    string Status,
    decimal SubTotalQAR,
    decimal TaxAmountQAR,
    decimal TotalQAR,
    string? Notes,
    string? TermsAndConditions,
    Guid? ConvertedToInvoiceId,
    IReadOnlyList<QuotationLineItemResponse> LineItems
);

public sealed record QuotationLineItemResponse(
    Guid Id,
    string Description,
    string ServiceType,
    decimal Quantity,
    decimal UnitPriceQAR,
    decimal DiscountPercent,
    decimal TaxPercent,
    decimal LineTotalQAR,
    int SortOrder
);
