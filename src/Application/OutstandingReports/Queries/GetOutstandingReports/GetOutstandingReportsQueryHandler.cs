using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.OutstandingReports.Queries.GetOutstandingReports;

internal sealed class GetOutstandingReportsQueryHandler(
    IApplicationDbContext dbContext)
    : IQueryHandler<GetOutstandingReportsQuery, IReadOnlyList<OutstandingReportResponse>>
{
    public async Task<Result<IReadOnlyList<OutstandingReportResponse>>> Handle(GetOutstandingReportsQuery request, CancellationToken cancellationToken)
    {
        IQueryable<Domain.Billing.OutstandingInvoiceReport> query = dbContext.OutstandingInvoiceReports.AsNoTracking();

        if (request.ClientId.HasValue)
        {
            query = query.Where(r => r.ClientId == request.ClientId.Value);
        }

        if (request.PeriodMonth.HasValue)
        {
            query = query.Where(r => r.PeriodMonth == request.PeriodMonth.Value);
        }

        if (request.PeriodYear.HasValue)
        {
            query = query.Where(r => r.PeriodYear == request.PeriodYear.Value);
        }

        List<OutstandingReportResponse> reports = await query
            .OrderByDescending(r => r.GeneratedAt)
            .Select(r => new OutstandingReportResponse(
                r.Id,
                r.ClientId,
                r.Client!.CompanyName,
                r.GeneratedAt,
                r.PeriodMonth,
                r.PeriodYear,
                r.TotalOutstandingQAR,
                r.InvoiceCount,
                r.OldestInvoiceDate,
                r.DeliveryStatus.ToString(),
                r.SentAt
            ))
            .ToListAsync(cancellationToken);

        return reports;
    }
}
