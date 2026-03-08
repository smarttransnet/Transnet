using Application.Abstractions.Messaging;

namespace Application.Payroll.Commands.CreateSalaryRecord;

public sealed record CreateSalaryRecordCommand(
    Guid DriverId,
    int PeriodMonth,
    int PeriodYear,
    decimal BaseSalaryQAR,
    decimal AllowancesQAR,
    string? Notes
) : ICommand<Guid>;
