namespace Application.ClientPortalAuth.Commands.Login;

public sealed record LoginResponse(
    string AccessToken,
    string RefreshToken,
    Guid UserId,
    Guid ClientId,
    string FullName,
    string Role
);
