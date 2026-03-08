using Application.Abstractions.Messaging;
using Domain.Drivers.Enums;

namespace Application.Drivers.Expenses.ReviewExpense;

public sealed record ReviewExpenseCommand(
    Guid ExpenseId,
    ExpenseStatus Status,
    string? ReviewNotes = null) : ICommand;
