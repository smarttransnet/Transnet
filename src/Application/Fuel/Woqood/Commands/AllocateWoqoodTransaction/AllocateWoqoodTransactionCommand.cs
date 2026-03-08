using Application.Abstractions.Messaging;

namespace Application.Fuel.Woqood.Commands.AllocateWoqoodTransaction;

public sealed record AllocateWoqoodTransactionCommand(
    Guid TransactionId,
    Guid VehicleId,
    Guid? TripId,
    Guid AllocatedByUserId,
    string? Notes
) : ICommand;
