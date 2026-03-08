using Domain.Payroll.Enums;
using Domain.Trips;
using SharedKernel;

namespace Domain.Payroll;

public sealed class DriverCommissionItem : Entity
{
    public Guid Id { get; set; }
    public Guid DriverSalaryRecordId { get; set; }
    public Guid? TripId { get; set; }
    public CommissionType CommissionType { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal AmountQAR { get; set; }
    public string? CalculationBasis { get; set; }
    public DateTime AppliedAt { get; set; }

    // Navigation Properties
    public DriverSalaryRecord? SalaryRecord { get; set; }
    public Trip? Trip { get; set; }
}
