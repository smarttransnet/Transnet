using Application.Abstractions.Messaging;
using Domain.Payroll.Enums;

namespace Application.Payroll.Commands.AddCommissionItem;

public sealed record AddCommissionItemCommand(
    Guid DriverSalaryRecordId,
    Guid? TripId,
    CommissionType CommissionType,
    string Description,
    decimal AmountQAR,
    string? CalculationBasis
) : ICommand<Guid>;
