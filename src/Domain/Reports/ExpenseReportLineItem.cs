using Domain.Reports.Enums;
using SharedKernel;

namespace Domain.Reports;

public sealed class ExpenseReportLineItem : Entity
{
    public Guid Id { get; set; }
    public Guid MonthlyExpenseReportId { get; set; }
    public ExpenseCategory Category { get; set; }
    public string? SubCategory { get; set; }
    public Guid? EntityId { get; set; }
    public string? EntityType { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal AmountQAR { get; set; }
    public int SortOrder { get; set; }

    // Navigation Properties
    public MonthlyExpenseReport? Report { get; set; }
}
