using Application.Abstractions.Messaging;

namespace Application.Reports.Queries.GetMonthlyExpenseReports;

public sealed record GetMonthlyExpenseReportsQuery(
    int? PeriodMonth = null,
    int? PeriodYear = null
) : IQuery<IReadOnlyList<MonthlyExpenseReportResponse>>;
