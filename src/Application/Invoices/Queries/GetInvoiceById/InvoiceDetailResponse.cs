namespace Application.Invoices.Queries.GetInvoiceById;

public sealed record InvoiceDetailResponse(
    Guid Id,
    string InvoiceNumber,
    Guid ClientId,
    string ClientName,
    Guid? QuotationId,
    DateTime IssuedAt,
    DateOnly DueDate,
    string Status,
    decimal SubTotalQAR,
    decimal TaxAmountQAR,
    decimal TotalQAR,
    decimal PaidAmountQAR,
    decimal OutstandingAmountQAR,
    string? Notes,
    IReadOnlyList<InvoiceLineItemResponse> LineItems,
    IReadOnlyList<InvoiceTripLinkResponse> TripLinks,
    IReadOnlyList<InvoicePaymentResponse> Payments
);

public sealed record InvoiceLineItemResponse(
    Guid Id,
    string Description,
    string ServiceType,
    decimal Quantity,
    decimal UnitPriceQAR,
    decimal DiscountPercent,
    decimal TaxPercent,
    decimal LineTotalQAR,
    int SortOrder,
    Guid? TripId
);

public sealed record InvoiceTripLinkResponse(
    Guid Id,
    Guid TripId,
    string TripNumber,
    bool CompletionVerified
);

public sealed record InvoicePaymentResponse(
    Guid Id,
    decimal AmountQAR,
    string PaymentMethod,
    string? PaymentReference,
    DateOnly PaymentDate,
    string? Notes
);
