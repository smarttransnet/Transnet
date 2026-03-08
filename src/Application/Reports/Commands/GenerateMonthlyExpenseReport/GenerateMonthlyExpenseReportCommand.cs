using Application.Abstractions.Messaging;

namespace Application.Reports.Commands.GenerateMonthlyExpenseReport;

public sealed record GenerateMonthlyExpenseReportCommand(
    int PeriodMonth,
    int PeriodYear,
    Guid UserId
) : ICommand<Guid>;
