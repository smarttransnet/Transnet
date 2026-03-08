using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Reports.Queries.GetMonthlyExpenseReports;

internal sealed class GetMonthlyExpenseReportsQueryHandler(
    IApplicationDbContext dbContext)
    : IQueryHandler<GetMonthlyExpenseReportsQuery, IReadOnlyList<MonthlyExpenseReportResponse>>
{
    public async Task<Result<IReadOnlyList<MonthlyExpenseReportResponse>>> Handle(GetMonthlyExpenseReportsQuery request, CancellationToken cancellationToken)
    {
        IQueryable<Domain.Reports.MonthlyExpenseReport> query = dbContext.MonthlyExpenseReports.AsQueryable();

        if (request.PeriodMonth.HasValue)
        {
            query = query.Where(r => r.PeriodMonth == request.PeriodMonth.Value);
        }

        if (request.PeriodYear.HasValue)
        {
            query = query.Where(r => r.PeriodYear == request.PeriodYear.Value);
        }

        List<MonthlyExpenseReportResponse> reports = await query
            .OrderByDescending(r => r.PeriodYear).ThenByDescending(r => r.PeriodMonth)
            .Select(r => new MonthlyExpenseReportResponse(
                r.Id,
                r.PeriodMonth,
                r.PeriodYear,
                r.ReportType.ToString(),
                r.TotalFuelCostQAR,
                r.TotalSalaryCostQAR,
                r.TotalDriverExpensesQAR,
                r.TotalOperationalCostQAR,
                r.Status.ToString(),
                r.ExportedFileUrl,
                r.GeneratedAt
            ))
            .ToListAsync(cancellationToken);

        return reports;
    }
}
