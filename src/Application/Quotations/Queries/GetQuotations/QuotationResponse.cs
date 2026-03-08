namespace Application.Quotations.Queries.GetQuotations;

public sealed record QuotationResponse(
    Guid Id,
    string QuotationNumber,
    Guid ClientId,
    string ClientName,
    DateTime IssuedAt,
    DateOnly ValidUntil,
    string Status,
    decimal TotalQAR
);
