using Application.Abstractions.Messaging;

namespace Application.Fuel.Woqood.Commands.UpsertWoqoodCardMapping;

public sealed record UpsertWoqoodCardMappingCommand(
    string WoqoodCardNumber,
    Guid? VehicleId,
    Guid? DriverId,
    string CardHolderName,
    string? Notes,
    bool IsActive,
    Guid UserId
) : ICommand<Guid>;
