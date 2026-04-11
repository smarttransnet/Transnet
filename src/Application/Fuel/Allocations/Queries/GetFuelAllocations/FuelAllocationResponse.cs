using Domain.Fuel.Enums;

namespace Application.Fuel.Allocations.Queries.GetFuelAllocations;

public sealed record FuelAllocationResponse(
    Guid Id,
    Guid? WoqoodFuelTransactionId,
    Guid? DriverExpenseId,
    Guid VehicleId,
    string? VehiclePlate,
    Guid? TripId,
    string AllocationSource,
    decimal Liters,
    decimal Amount,
    string FuelType,
    DateOnly Date,
    string? Remarks
);
