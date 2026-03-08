using Application.Abstractions.Messaging;

namespace Application.OutstandingReports.Queries.GetOutstandingReports;

public sealed record GetOutstandingReportsQuery(
    Guid? ClientId = null,
    int? PeriodMonth = null,
    int? PeriodYear = null
) : IQuery<IReadOnlyList<OutstandingReportResponse>>;
