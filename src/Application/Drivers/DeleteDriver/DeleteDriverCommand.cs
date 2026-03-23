using Application.Abstractions.Messaging;

namespace Application.Drivers.DeleteDriver;

public sealed record DeleteDriverCommand(Guid Id) : ICommand;
