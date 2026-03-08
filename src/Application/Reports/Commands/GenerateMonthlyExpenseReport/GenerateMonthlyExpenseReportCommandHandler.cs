using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Reports;
using Domain.Reports.Enums;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Reports.Commands.GenerateMonthlyExpenseReport;

internal sealed class GenerateMonthlyExpenseReportCommandHandler(
    IApplicationDbContext dbContext)
    : ICommandHandler<GenerateMonthlyExpenseReportCommand, Guid>
{
    public async Task<Result<Guid>> Handle(GenerateMonthlyExpenseReportCommand request, CancellationToken cancellationToken)
    {
        Domain.Reports.MonthlyExpenseReport? existing = await dbContext.MonthlyExpenseReports
            .FirstOrDefaultAsync(r => r.PeriodYear == request.PeriodYear 
                                   && r.PeriodMonth == request.PeriodMonth
                                   && r.ReportType == ExpenseReportType.Monthly, 
                                 cancellationToken);

        if (existing is not null)
        {
            return existing.Id; // Or return Error.Conflict
        }

        // Scaffolded logic to just create the draft report headers:
        var report = new MonthlyExpenseReport
        {
            Id = Guid.NewGuid(),
            PeriodMonth = request.PeriodMonth,
            PeriodYear = request.PeriodYear,
            ReportType = ExpenseReportType.Monthly,
            GeneratedByUserId = request.UserId,
            GeneratedAt = DateTime.UtcNow,
            TotalFuelCostQAR = 0,
            TotalSalaryCostQAR = 0,
            TotalDriverExpensesQAR = 0,
            TotalOperationalCostQAR = 0,
            Status = ReportStatus.Draft
        };

        dbContext.MonthlyExpenseReports.Add(report);
        await dbContext.SaveChangesAsync(cancellationToken);

        // Here we'd queue a background task to sum up items and create line items.

        return report.Id;
    }
}
