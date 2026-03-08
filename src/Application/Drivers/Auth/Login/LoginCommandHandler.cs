using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Drivers;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Drivers.Auth.Login;

internal sealed class LoginCommandHandler(
    IApplicationDbContext dbContext,
    IPasswordHasher passwordHasher,
    ITokenProvider tokenProvider) : ICommandHandler<LoginCommand, LoginResponse>
{
    public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        Driver? driver = await dbContext.Drivers
            .FirstOrDefaultAsync(d => d.EmployeeNumber == request.Username, cancellationToken);

        if (driver is null)
        {
            return Result.Failure<LoginResponse>(Error.Problem("Auth.Failed", "Invalid credentials."));
        }

        if (!driver.IsActive)
        {
            return Result.Failure<LoginResponse>(Error.Problem("Auth.Inactive", "Account is inactive."));
        }

        DriverAuthCredential? credential = await dbContext.DriverAuthCredentials
            .FirstOrDefaultAsync(c => c.DriverId == driver.Id, cancellationToken);

        if (credential is null)
        {
            return Result.Failure<LoginResponse>(Error.Problem("Auth.Failed", "Invalid credentials."));
        }

        if (credential.IsLocked)
        {
            return Result.Failure<LoginResponse>(Error.Problem("Auth.LockedOut", "Account is locked due to too many failed attempts."));
        }

        bool isPasswordValid = passwordHasher.Verify(request.Password, credential.PasswordHash);

        if (!isPasswordValid)
        {
            credential.FailedAttempts++;
            if (credential.FailedAttempts >= 5)
            {
                credential.IsLocked = true;
                
            }
            await dbContext.SaveChangesAsync(cancellationToken);
            return Result.Failure<LoginResponse>(Error.Problem("Auth.Failed", "Invalid credentials."));
        }

        credential.FailedAttempts = 0;
        credential.IsLocked = false;
        
        credential.LastLoginAt = DateTime.UtcNow;

        if (!string.IsNullOrWhiteSpace(request.DeviceToken))
        {
            credential.DeviceToken = request.DeviceToken;
        }

        string accessToken = tokenProvider.CreateForDriver(driver);
        string refreshToken = tokenProvider.CreateRefreshToken();

        credential.RefreshToken = refreshToken;
        credential.RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(7);

        await dbContext.SaveChangesAsync(cancellationToken);

        return new LoginResponse(accessToken, refreshToken, driver.Id, driver.FirstName, driver.LastName);
    }
}

