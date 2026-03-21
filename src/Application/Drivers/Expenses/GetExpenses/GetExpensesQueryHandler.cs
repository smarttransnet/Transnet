using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Drivers;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Drivers.Expenses.GetExpenses;

internal sealed class GetExpensesQueryHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetExpensesQuery, PagedList<ExpenseResponse>>
{
    public async Task<Result<PagedList<ExpenseResponse>>> Handle(GetExpensesQuery request, CancellationToken cancellationToken)
    {
        IQueryable<DriverExpense> expensesQuery = dbContext.DriverExpenses;

        if (request.DriverId.HasValue)
        {
            expensesQuery = expensesQuery.Where(e => e.DriverId == request.DriverId.Value);
        }

        if (request.TripId.HasValue)
        {
            expensesQuery = expensesQuery.Where(e => e.TripId == request.TripId.Value);
        }

        if (request.Status.HasValue)
        {
            expensesQuery = expensesQuery.Where(e => e.Status == request.Status.Value);
        }

        if (request.StartDate.HasValue)
        {
            expensesQuery = expensesQuery.Where(e => e.ExpenseDate >= request.StartDate.Value);
        }

        if (request.EndDate.HasValue)
        {
            expensesQuery = expensesQuery.Where(e => e.ExpenseDate <= request.EndDate.Value);
        }

        int totalCount = await expensesQuery.CountAsync(cancellationToken);

        List<ExpenseResponse> expenses = await expensesQuery
            .OrderByDescending(e => e.ExpenseDate)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new ExpenseResponse(
                e.Id,
                e.DriverId,
                e.TripId,
                e.ExpenseType,
                e.AmountQAR,
                e.ExpenseDate,
                e.Description,
                e.ReceiptUrl != null ? new Uri(e.ReceiptUrl) : null,
                e.FuelLitres,
                e.FuelStation,
                e.OdometerReading,
                e.Status,
                e.ReviewedByUserId,
                e.ReviewedAt,
                e.SubmittedAt))
            .ToListAsync(cancellationToken);

        return new PagedList<ExpenseResponse>(expenses, totalCount, request.Page, request.PageSize);
    }
}
