using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Drivers;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Drivers.Auth.Refresh;

internal sealed class RefreshCommandHandler(
    IApplicationDbContext dbContext,
    ITokenProvider tokenProvider) : ICommandHandler<RefreshCommand, RefreshResponse>
{
    public async Task<Result<RefreshResponse>> Handle(RefreshCommand request, CancellationToken cancellationToken)
    {
        Driver? driver = await dbContext.Drivers
            .FirstOrDefaultAsync(d => d.Id == request.DriverId, cancellationToken);

        if (driver is null || !driver.IsActive)
        {
            return Result.Failure<RefreshResponse>(Error.Problem("Auth.Invalid", "Invalid or inactive user."));
        }

        DriverAuthCredential? credential = await dbContext.DriverAuthCredentials
            .FirstOrDefaultAsync(c => c.DriverId == request.DriverId, cancellationToken);

        if (credential is null || credential.RefreshToken != request.RefreshToken || credential.RefreshTokenExpiresAt < DateTime.UtcNow)
        {
            return Result.Failure<RefreshResponse>(Error.Problem("Auth.InvalidToken", "Invalid or expired refresh token."));
        }

        if (credential.IsLocked)
        {
            return Result.Failure<RefreshResponse>(Error.Problem("Auth.LockedOut", "Account is locked out."));
        }

        string accessToken = tokenProvider.CreateForDriver(driver);
        string newRefreshToken = tokenProvider.CreateRefreshToken();

        credential.RefreshToken = newRefreshToken;
        credential.RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(7);

        await dbContext.SaveChangesAsync(cancellationToken);

        return new RefreshResponse(accessToken, newRefreshToken);
    }
}

