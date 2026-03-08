using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Payroll;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Payroll.Commands.AddCommissionItem;

internal sealed class AddCommissionItemCommandHandler(
    IApplicationDbContext dbContext)
    : ICommandHandler<AddCommissionItemCommand, Guid>
{
    public async Task<Result<Guid>> Handle(AddCommissionItemCommand request, CancellationToken cancellationToken)
    {
        Domain.Payroll.DriverSalaryRecord? record = await dbContext.DriverSalaryRecords
            .FirstOrDefaultAsync(s => s.Id == request.DriverSalaryRecordId, cancellationToken);

        if (record is null)
        {
            return Result.Failure<Guid>(Error.NotFound("DriverSalaryRecord.NotFound", "The salary record was not found."));
        }

        var item = new DriverCommissionItem
        {
            Id = Guid.NewGuid(),
            DriverSalaryRecordId = request.DriverSalaryRecordId,
            TripId = request.TripId,
            CommissionType = request.CommissionType,
            Description = request.Description,
            AmountQAR = request.AmountQAR,
            CalculationBasis = request.CalculationBasis,
            AppliedAt = DateTime.UtcNow
        };

        record.CommissionQAR += request.AmountQAR;
        record.NetPayableQAR += request.AmountQAR;

        dbContext.DriverCommissionItems.Add(item);
        
        await dbContext.SaveChangesAsync(cancellationToken);

        return item.Id;
    }
}
