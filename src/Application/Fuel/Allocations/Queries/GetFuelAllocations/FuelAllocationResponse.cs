using Domain.Fuel.Enums;

namespace Application.Fuel.Allocations.Queries.GetFuelAllocations;

public sealed record FuelAllocationResponse(
    Guid Id,
    Guid? WoqoodFuelTransactionId,
    Guid? DriverExpenseId,
    Guid VehicleId,
    Guid? TripId,
    string AllocationSource,
    decimal QuantityLitres,
    decimal AmountQAR,
    DateOnly AllocationDate,
    string? Notes
);
