using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Payroll.Enums;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Payroll.Commands.SubmitSalaryRecord;

internal sealed class SubmitSalaryRecordCommandHandler(
    IApplicationDbContext dbContext)
    : ICommandHandler<SubmitSalaryRecordCommand>
{
    public async Task<Result> Handle(SubmitSalaryRecordCommand request, CancellationToken cancellationToken)
    {
        Domain.Payroll.DriverSalaryRecord? record = await dbContext.DriverSalaryRecords
            .FirstOrDefaultAsync(s => s.Id == request.SalaryRecordId, cancellationToken);

        if (record is null)
        {
            return Result.Failure(Error.NotFound("DriverSalaryRecord.NotFound", "The salary record was not found."));
        }

        if (record.SponsorApprovalStatus != ApprovalStatus.Draft && record.SponsorApprovalStatus != ApprovalStatus.Rejected)
        {
            return Result.Failure(Error.Conflict("DriverSalaryRecord.InvalidStatus", "Record must be in Draft or Rejected status to be submitted."));
        }

        record.SponsorApprovalStatus = ApprovalStatus.PendingApproval;

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
