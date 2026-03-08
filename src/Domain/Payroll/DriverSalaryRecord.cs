using Domain.Drivers;
using Domain.Payroll.Enums;
using SharedKernel;

namespace Domain.Payroll;

public sealed class DriverSalaryRecord : Entity
{
    public Guid Id { get; set; }
    public Guid DriverId { get; set; }
    public int PeriodMonth { get; set; }
    public int PeriodYear { get; set; }
    public decimal BaseSalaryQAR { get; set; }
    public decimal AllowancesQAR { get; set; }
    public decimal OvertimeQAR { get; set; }
    public decimal DeductionsQAR { get; set; }
    public decimal CommissionQAR { get; set; }
    public decimal NetPayableQAR { get; set; }
    public ApprovalStatus SponsorApprovalStatus { get; set; }
    public DateTime? SponsorApprovedAt { get; set; }
    public Guid? SponsorApprovedById { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation Properties
    public Driver? Driver { get; set; }
    public ICollection<DriverCommissionItem> CommissionItems { get; set; } = [];
    public ICollection<SalaryExpenseLine> ExpenseLines { get; set; } = [];
}
