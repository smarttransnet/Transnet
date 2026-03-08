using Application.Abstractions.Messaging;
using Domain.Drivers;
using Domain.Drivers.Enums;
namespace Application.Drivers.Expenses.GetExpenses; public sealed record ExpenseResponse(Guid Id, Guid DriverId, Guid? TripId, ExpenseType ExpenseType, decimal AmountQAR, DateOnly ExpenseDate, string? Description, Uri? ReceiptUrl, decimal? FuelLitres, string? FuelStation, decimal? OdometerReading, ExpenseStatus Status, Guid? ReviewedByUserId, DateTime? ReviewedAt, DateTime SubmittedAt);
