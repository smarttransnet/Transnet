using Domain.Reports.Enums;
using SharedKernel;

namespace Domain.Reports;

public sealed class MonthlyExpenseReport : Entity
{
    public Guid Id { get; set; }
    public int PeriodMonth { get; set; }
    public int PeriodYear { get; set; }
    public ExpenseReportType ReportType { get; set; }
    public Guid GeneratedByUserId { get; set; }
    public DateTime GeneratedAt { get; set; }
    public decimal TotalFuelCostQAR { get; set; }
    public decimal TotalSalaryCostQAR { get; set; }
    public decimal TotalDriverExpensesQAR { get; set; }
    public decimal TotalOperationalCostQAR { get; set; }
    public ReportStatus Status { get; set; }
    public string? ExportedFileUrl { get; set; }
    public string? Notes { get; set; }

    // Navigation Properties
    public ICollection<ExpenseReportLineItem> LineItems { get; set; } = [];
}
