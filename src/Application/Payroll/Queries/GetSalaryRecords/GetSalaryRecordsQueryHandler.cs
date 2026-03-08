using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Payroll.Queries.GetSalaryRecords;

internal sealed class GetSalaryRecordsQueryHandler(
    IApplicationDbContext dbContext)
    : IQueryHandler<GetSalaryRecordsQuery, IReadOnlyList<SalaryRecordResponse>>
{
    public async Task<Result<IReadOnlyList<SalaryRecordResponse>>> Handle(GetSalaryRecordsQuery request, CancellationToken cancellationToken)
    {
        IQueryable<Domain.Payroll.DriverSalaryRecord> query = dbContext.DriverSalaryRecords.AsQueryable();

        if (request.DriverId.HasValue)
        {
            query = query.Where(s => s.DriverId == request.DriverId.Value);
        }

        if (request.PeriodMonth.HasValue)
        {
            query = query.Where(s => s.PeriodMonth == request.PeriodMonth.Value);
        }

        if (request.PeriodYear.HasValue)
        {
            query = query.Where(s => s.PeriodYear == request.PeriodYear.Value);
        }

        List<SalaryRecordResponse> records = await query
            .OrderByDescending(s => s.PeriodYear).ThenByDescending(s => s.PeriodMonth)
            .Select(s => new SalaryRecordResponse(
                s.Id,
                s.DriverId,
                s.PeriodMonth,
                s.PeriodYear,
                s.BaseSalaryQAR,
                s.AllowancesQAR,
                s.OvertimeQAR,
                s.DeductionsQAR,
                s.CommissionQAR,
                s.NetPayableQAR,
                s.SponsorApprovalStatus.ToString()
            ))
            .ToListAsync(cancellationToken);

        return records;
    }
}
