using Application.Abstractions.Messaging;
using Domain.Clients.Enums;

namespace Application.Clients.Commands.CreatePortalUser;

public sealed record CreatePortalUserCommand(
    Guid ClientId,
    string FullName,
    string Email,
    string PlainTextPassword,
    ClientPortalRole Role
) : ICommand<Guid>;
