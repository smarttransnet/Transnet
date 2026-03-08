using Application.Abstractions.Messaging;

namespace Application.Fuel.Allocations.Commands.CreateFuelAllocation;

public sealed record CreateFuelAllocationCommand(
    Guid VehicleId,
    Guid? TripId,
    decimal QuantityLitres,
    decimal AmountQAR,
    DateOnly AllocationDate,
    string? Notes,
    Guid UserId
) : ICommand<Guid>;
