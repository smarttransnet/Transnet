using Application.Abstractions.Messaging;

namespace Application.Fuel.Allocations.Commands.UpdateFuelAllocation;

public sealed record UpdateFuelAllocationCommand(
    Guid Id,
    Guid VehicleId,
    Guid? TripId,
    decimal QuantityLitres,
    decimal AmountQAR,
    Domain.Fuel.Enums.FuelType FuelType,
    DateOnly AllocationDate,
    string? Notes,
    Guid UserId
) : ICommand;
