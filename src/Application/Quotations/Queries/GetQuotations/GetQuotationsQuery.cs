using Application.Abstractions.Messaging;
using Domain.Billing.Enums;

namespace Application.Quotations.Queries.GetQuotations;

public sealed record GetQuotationsQuery(
    Guid? ClientId = null,
    QuotationStatus? Status = null,
    string? SearchTerm = null
) : IQuery<IReadOnlyList<QuotationResponse>>;
