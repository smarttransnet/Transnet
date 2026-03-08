using Application.Abstractions.Messaging;
using Domain.Drivers.Enums;

namespace Application.Drivers.Expenses.SubmitExpense;

public sealed record SubmitExpenseCommand(
    Guid DriverId,
    Guid? TripId,
    ExpenseType ExpenseType,
    decimal Amount,
    DateOnly ExpenseDate,
    string Description,
    Uri? ReceiptUrl = null,
    decimal? FuelLitres = null,
    string? FuelStation = null,
    decimal? OdometerReading = null) : ICommand<Guid>;
