using Application.Abstractions.Messaging;

namespace Application.Clients.DeleteClient;

public sealed record DeleteClientCommand(Guid Id) : ICommand;
