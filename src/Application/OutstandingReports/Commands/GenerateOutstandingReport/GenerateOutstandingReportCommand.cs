using Application.Abstractions.Messaging;

namespace Application.OutstandingReports.Commands.GenerateOutstandingReport;

public sealed record GenerateOutstandingReportCommand(
    Guid ClientId,
    int PeriodMonth,
    int PeriodYear
) : ICommand<Guid>;
