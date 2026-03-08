using Domain.Drivers;
using Domain.Payroll.Enums;
using SharedKernel;

namespace Domain.Payroll;

public sealed class SalaryExpenseLine : Entity
{
    public Guid Id { get; set; }
    public Guid DriverSalaryRecordId { get; set; }
    public Guid? DriverExpenseId { get; set; }
    public SalaryExpenseLineType LineType { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal AmountQAR { get; set; }
    public bool IsDeduction { get; set; }

    // Navigation Properties
    public DriverSalaryRecord? SalaryRecord { get; set; }
    public DriverExpense? DriverExpense { get; set; }
}
