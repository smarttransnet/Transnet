using Application.Abstractions.Messaging;
using Domain.Clients.Enums;

namespace Application.Clients.Commands.UpdatePortalUser;

public sealed record UpdatePortalUserCommand(
    Guid UserId,
    Guid ClientId,
    string FullName,
    string Email,
    ClientPortalRole Role,
    bool IsActive
) : ICommand;
