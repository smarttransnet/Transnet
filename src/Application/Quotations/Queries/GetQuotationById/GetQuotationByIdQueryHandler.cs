using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Quotations.Queries.GetQuotationById;

internal sealed class GetQuotationByIdQueryHandler(
    IApplicationDbContext dbContext)
    : IQueryHandler<GetQuotationByIdQuery, QuotationDetailResponse>
{
    public async Task<Result<QuotationDetailResponse>> Handle(GetQuotationByIdQuery request, CancellationToken cancellationToken)
    {
        QuotationDetailResponse? quotation = await dbContext.Quotations
            .AsNoTracking()
            .Where(q => q.Id == request.QuotationId)
            .Select(q => new QuotationDetailResponse(
                q.Id,
                q.QuotationNumber,
                q.ClientId,
                q.Client!.CompanyName,
                q.IssuedAt,
                q.ValidUntil,
                q.Status.ToString(),
                q.SubTotalQAR,
                q.TaxAmountQAR,
                q.TotalQAR,
                q.Notes,
                q.TermsAndConditions,
                q.ConvertedToInvoiceId,
                q.LineItems.OrderBy(li => li.SortOrder).Select(li => new QuotationLineItemResponse(
                    li.Id,
                    li.Description,
                    li.ServiceType.ToString(),
                    li.Quantity,
                    li.UnitPriceQAR,
                    li.DiscountPercent,
                    li.TaxPercent,
                    li.LineTotalQAR,
                    li.SortOrder
                )).ToList()
            ))
            .FirstOrDefaultAsync(cancellationToken);

        if (quotation is null)
        {
            return Result.Failure<QuotationDetailResponse>(Error.NotFound("Quotation.NotFound", "The requested quotation was not found."));
        }

        return quotation;
    }
}
