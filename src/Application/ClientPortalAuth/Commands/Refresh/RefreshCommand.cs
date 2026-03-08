using Application.Abstractions.Messaging;

namespace Application.ClientPortalAuth.Commands.Refresh;

public sealed record RefreshCommand(
    Guid UserId,
    Guid ClientId,
    string RefreshToken
) : ICommand<RefreshResponse>;
