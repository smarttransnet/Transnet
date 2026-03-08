namespace Application.Payroll.Queries.GetSalaryRecords;

public sealed record SalaryRecordResponse(
    Guid Id,
    Guid DriverId,
    int PeriodMonth,
    int PeriodYear,
    decimal BaseSalaryQAR,
    decimal AllowancesQAR,
    decimal OvertimeQAR,
    decimal DeductionsQAR,
    decimal CommissionQAR,
    decimal NetPayableQAR,
    string SponsorApprovalStatus
);
