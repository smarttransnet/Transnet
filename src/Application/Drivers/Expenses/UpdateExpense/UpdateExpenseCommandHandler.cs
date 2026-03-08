using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Drivers;
using Domain.Drivers.Enums;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Drivers.Expenses.UpdateExpense;

internal sealed class UpdateExpenseCommandHandler(IApplicationDbContext dbContext)
    : ICommandHandler<UpdateExpenseCommand>
{
    public async Task<Result> Handle(UpdateExpenseCommand request, CancellationToken cancellationToken)
    {
        DriverExpense? expense = await dbContext.DriverExpenses
            .FirstOrDefaultAsync(e => e.Id == request.ExpenseId, cancellationToken);

        if (expense is null)
        {
            return Result.Failure(Error.NotFound("Expenses.NotFound", "The expense record was not found."));
        }

        if (expense.Status != ExpenseStatus.Submitted)
        {
            return Result.Failure(Error.Conflict("Expenses.NotPending", "Only pending expenses can be updated."));
        }

        expense.AmountQAR = request.Amount;
        expense.ExpenseDate = request.ExpenseDate;
        expense.Description = request.Description;
        expense.ReceiptUrl = request.ReceiptUrl?.ToString();

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
