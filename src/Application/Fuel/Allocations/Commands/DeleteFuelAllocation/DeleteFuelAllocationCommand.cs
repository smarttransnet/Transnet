using Application.Abstractions.Messaging;

namespace Application.Fuel.Allocations.Commands.DeleteFuelAllocation;

public sealed record DeleteFuelAllocationCommand(Guid Id) : ICommand;
