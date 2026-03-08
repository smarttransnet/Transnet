using Application.Abstractions.Messaging;

namespace Application.Payroll.Queries.GetSalaryRecords;

public sealed record GetSalaryRecordsQuery(
    Guid? DriverId = null,
    int? PeriodMonth = null,
    int? PeriodYear = null
) : IQuery<IReadOnlyList<SalaryRecordResponse>>;
