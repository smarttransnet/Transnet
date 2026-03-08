using Application.Abstractions.Messaging;

namespace Application.ClientPortalAuth.Commands.Login;

public sealed record LoginCommand(
    string Email,
    string Password
) : ICommand<LoginResponse>;
