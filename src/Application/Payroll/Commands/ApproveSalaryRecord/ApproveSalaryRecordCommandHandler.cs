using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Payroll.Enums;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Payroll.Commands.ApproveSalaryRecord;

internal sealed class ApproveSalaryRecordCommandHandler(
    IApplicationDbContext dbContext)
    : ICommandHandler<ApproveSalaryRecordCommand>
{
    public async Task<Result> Handle(ApproveSalaryRecordCommand request, CancellationToken cancellationToken)
    {
        Domain.Payroll.DriverSalaryRecord? record = await dbContext.DriverSalaryRecords
            .FirstOrDefaultAsync(s => s.Id == request.SalaryRecordId, cancellationToken);

        if (record is null)
        {
            return Result.Failure(Error.NotFound("DriverSalaryRecord.NotFound", "The salary record was not found."));
        }

        if (record.SponsorApprovalStatus != ApprovalStatus.PendingApproval)
        {
            return Result.Failure(Error.Conflict("DriverSalaryRecord.InvalidStatus", "Record must be in PendingApproval status to be approved."));
        }

        record.SponsorApprovalStatus = ApprovalStatus.Approved;
        record.SponsorApprovedAt = DateTime.UtcNow;
        record.SponsorApprovedById = request.SponsorId;

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
