namespace Application.ClientPortalAuth.Commands.Refresh;

public sealed record RefreshResponse(
    string AccessToken,
    string RefreshToken
);
