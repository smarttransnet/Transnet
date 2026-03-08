using Application.Abstractions.Messaging;

namespace Application.Quotations.Queries.GetQuotationById;

public sealed record GetQuotationByIdQuery(Guid QuotationId) : IQuery<QuotationDetailResponse>;
