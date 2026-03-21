using Application.Abstractions.Messaging;

namespace Application.Fuel.Woqood.Commands.DeleteWoqoodCardMapping;

public sealed record DeleteWoqoodCardMappingCommand(Guid Id) : ICommand;
