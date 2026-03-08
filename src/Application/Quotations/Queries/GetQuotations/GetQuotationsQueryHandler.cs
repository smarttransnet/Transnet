using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Quotations.Queries.GetQuotations;

internal sealed class GetQuotationsQueryHandler(
    IApplicationDbContext dbContext)
    : IQueryHandler<GetQuotationsQuery, IReadOnlyList<QuotationResponse>>
{
    public async Task<Result<IReadOnlyList<QuotationResponse>>> Handle(GetQuotationsQuery request, CancellationToken cancellationToken)
    {
        IQueryable<Domain.Billing.Quotation> query = dbContext.Quotations.AsNoTracking();

        if (request.ClientId.HasValue)
        {
            query = query.Where(q => q.ClientId == request.ClientId.Value);
        }

        if (request.Status.HasValue)
        {
            query = query.Where(q => q.Status == request.Status.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            query = query.Where(q => q.QuotationNumber.Contains(request.SearchTerm));
        }

        List<QuotationResponse> quotations = await query
            .OrderByDescending(q => q.IssuedAt)
            .Select(q => new QuotationResponse(
                q.Id,
                q.QuotationNumber,
                q.ClientId,
                q.Client!.CompanyName,
                q.IssuedAt,
                q.ValidUntil,
                q.Status.ToString(),
                q.TotalQAR
            ))
            .ToListAsync(cancellationToken);

        return quotations;
    }
}
