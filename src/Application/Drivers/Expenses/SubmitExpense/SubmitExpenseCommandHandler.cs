using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Drivers;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Drivers.Expenses.SubmitExpense;

internal sealed class SubmitExpenseCommandHandler(
    IApplicationDbContext dbContext,
    IDateTimeProvider dateTimeProvider) : ICommandHandler<SubmitExpenseCommand, Guid>
{
    public async Task<Result<Guid>> Handle(SubmitExpenseCommand request, CancellationToken cancellationToken)
    {
        bool driverExists = await dbContext.Drivers.AnyAsync(d => d.Id == request.DriverId, cancellationToken);
        if (!driverExists)
        {
            return Result.Failure<Guid>(DriverErrors.NotFound(request.DriverId));
        }

        var expense = new DriverExpense
        {
            Id = Guid.NewGuid(),
            DriverId = request.DriverId,
            TripId = request.TripId,
            ExpenseType = request.ExpenseType,
            AmountQAR = request.Amount,
            ExpenseDate = request.ExpenseDate,
            Description = request.Description,
            ReceiptUrl = request.ReceiptUrl?.ToString(),
            FuelLitres = request.FuelLitres,
            FuelStation = request.FuelStation,
            OdometerReading = request.OdometerReading,
            Status = Domain.Drivers.Enums.ExpenseStatus.Submitted,
            SubmittedAt = dateTimeProvider.UtcNow
        };

        dbContext.DriverExpenses.Add(expense);
        await dbContext.SaveChangesAsync(cancellationToken);

        return expense.Id;
    }
}
