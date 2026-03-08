using Application.Abstractions.Messaging;

namespace Application.Fuel.Allocations.Queries.GetFuelAllocations;

public sealed record GetFuelAllocationsQuery(
    Guid? VehicleId = null,
    Guid? TripId = null,
    DateOnly? StartDate = null,
    DateOnly? EndDate = null
) : IQuery<IReadOnlyList<FuelAllocationResponse>>;
