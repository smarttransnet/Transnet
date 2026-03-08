
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Drivers;
using Domain.Drivers.Enums;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Drivers.Expenses.ReviewExpense;

internal sealed class ReviewExpenseCommandHandler(
    IApplicationDbContext dbContext,
    IDateTimeProvider dateTimeProvider) : ICommandHandler<ReviewExpenseCommand>
{
    public async Task<Result> Handle(ReviewExpenseCommand request, CancellationToken cancellationToken)
    {
        DriverExpense? expense = await dbContext.DriverExpenses
            .FirstOrDefaultAsync(e => e.Id == request.ExpenseId, cancellationToken);

        if (expense is null)
        {
            return Result.Failure(Error.NotFound("Expenses.NotFound", "The expense record was not found."));
        }

        if (request.Status != ExpenseStatus.Approved && request.Status != ExpenseStatus.Rejected)
        {
            return Result.Failure(Error.Conflict("Expenses.InvalidStatus", "Expenses can only be Approved or Rejected."));
        }

        expense.Status = request.Status;
        
        expense.ReviewedAt = dateTimeProvider.UtcNow;
        // In a real system, you might record ReviewedByUserId here if you have the current user context

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

