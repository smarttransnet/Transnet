using Application.Abstractions.Messaging;

namespace Application.Fuel.Allocations.Commands.CreateFuelAllocation;

public sealed record CreateFuelAllocationCommand(
    Guid VehicleId,
    Guid? TripId,
    decimal QuantityLitres,
    decimal AmountQAR,
    Domain.Fuel.Enums.FuelType FuelType,
    DateOnly AllocationDate,
    string? Notes,
    Guid UserId
) : ICommand<Guid>;
