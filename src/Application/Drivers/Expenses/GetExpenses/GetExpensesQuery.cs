using Application.Abstractions.Messaging;
using Domain.Drivers;
using Domain.Drivers.Enums;
using SharedKernel;
namespace Application.Drivers.Expenses.GetExpenses; public sealed record GetExpensesQuery(Guid? DriverId = null, Guid? TripId = null, ExpenseStatus? Status = null, DateOnly? StartDate = null, DateOnly? EndDate = null, int Page = 1, int PageSize = 10) : IQuery<PagedList<ExpenseResponse>>;
