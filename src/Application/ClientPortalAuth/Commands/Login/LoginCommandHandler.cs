using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Clients;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.ClientPortalAuth.Commands.Login;

internal sealed class LoginCommandHandler(
    IApplicationDbContext dbContext,
    IPasswordHasher passwordHasher,
    ITokenProvider tokenProvider)
    : ICommandHandler<LoginCommand, LoginResponse>
{
    public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        ClientPortalUser? user = await dbContext.ClientPortalUsers
            .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

        if (user is null)
        {
            return Result.Failure<LoginResponse>(Error.Problem("Auth.Failed", "Invalid credentials."));
        }

        if (!user.IsActive)
        {
            return Result.Failure<LoginResponse>(Error.Problem("Auth.Inactive", "Account is inactive."));
        }

        // Hash checking (assuming PasswordHash stores the hash and Salt might be used internally depending on implementation of IPasswordHasher)
        // For simplicity matching Driver Login:
        bool isPasswordValid = passwordHasher.Verify(request.Password, user.PasswordHash);

        if (!isPasswordValid)
        {
            return Result.Failure<LoginResponse>(Error.Problem("Auth.Failed", "Invalid credentials."));
        }

        user.LastLoginAt = DateTime.UtcNow;

        // Note: ITokenProvider needs CreateForClientPortalUser. If not available, we assume it gets added.
        string accessToken = tokenProvider.CreateForClientPortalUser(user);
        string refreshToken = tokenProvider.CreateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(7);

        await dbContext.SaveChangesAsync(cancellationToken);

        return new LoginResponse(
            accessToken,
            refreshToken,
            user.Id,
            user.ClientId,
            user.FullName,
            user.Role.ToString()
        );
    }
}
