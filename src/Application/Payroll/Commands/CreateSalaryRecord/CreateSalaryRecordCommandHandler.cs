using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Payroll;
using Domain.Payroll.Enums;
using SharedKernel;

namespace Application.Payroll.Commands.CreateSalaryRecord;

internal sealed class CreateSalaryRecordCommandHandler(
    IApplicationDbContext dbContext)
    : ICommandHandler<CreateSalaryRecordCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateSalaryRecordCommand request, CancellationToken cancellationToken)
    {
        var record = new DriverSalaryRecord
        {
            Id = Guid.NewGuid(),
            DriverId = request.DriverId,
            PeriodMonth = request.PeriodMonth,
            PeriodYear = request.PeriodYear,
            BaseSalaryQAR = request.BaseSalaryQAR,
            AllowancesQAR = request.AllowancesQAR,
            OvertimeQAR = 0,
            DeductionsQAR = 0,
            CommissionQAR = 0,
            NetPayableQAR = request.BaseSalaryQAR + request.AllowancesQAR,
            SponsorApprovalStatus = ApprovalStatus.Draft,
            Notes = request.Notes,
            CreatedAt = DateTime.UtcNow
        };

        dbContext.DriverSalaryRecords.Add(record);
        await dbContext.SaveChangesAsync(cancellationToken);

        return record.Id;
    }
}
