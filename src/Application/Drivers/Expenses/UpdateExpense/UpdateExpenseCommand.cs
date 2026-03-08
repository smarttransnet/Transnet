using Application.Abstractions.Messaging;

namespace Application.Drivers.Expenses.UpdateExpense;

public sealed record UpdateExpenseCommand(
    Guid ExpenseId,
    decimal Amount,
    DateOnly ExpenseDate,
    string Description,
    Uri? ReceiptUrl = null) : ICommand;
