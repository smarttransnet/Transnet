using Application.Abstractions.Messaging;

namespace Application.Fuel.Woqood.Commands.DeactivateWoqoodCardMapping;

public sealed record DeactivateWoqoodCardMappingCommand(Guid MappingId) : ICommand;
