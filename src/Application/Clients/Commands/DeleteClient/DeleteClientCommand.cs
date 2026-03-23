using Application.Abstractions.Messaging;

namespace Application.Clients.Commands.DeleteClient;

public sealed record DeleteClientCommand(Guid Id) : ICommand;
