using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Drivers;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Drivers.Auth.CreateCredentials;

internal sealed class CreateCredentialsCommandHandler(
    IApplicationDbContext dbContext,
    IPasswordHasher passwordHasher,
    IDateTimeProvider dateTimeProvider)
    : ICommandHandler<CreateCredentialsCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateCredentialsCommand request, CancellationToken cancellationToken)
    {
        Driver? driver = await dbContext.Drivers
            .FirstOrDefaultAsync(d => d.Id == request.DriverId, cancellationToken);

        if (driver is null)
        {
            return Result.Failure<Guid>(Error.NotFound("Drivers.NotFound", $"The driver with the Id = '{request.DriverId}' was not found"));
        }

        if (await dbContext.DriverAuthCredentials.AnyAsync(c => c.DriverId == request.DriverId, cancellationToken))
        {
            return Result.Failure<Guid>(Error.Conflict("Drivers.CredentialsAlreadyExist", "Credentials already exist for this driver"));
        }

        var credentials = new DriverAuthCredential
        {
            Id = Guid.NewGuid(),
            DriverId = request.DriverId,
            UsernameHash = passwordHasher.Hash(driver.EmployeeNumber), // Assuming EmployeeNumber as username
            PasswordHash = passwordHasher.Hash(request.Password),
            Platform = request.Platform,
            DeviceToken = request.DeviceToken,
            IsLocked = false,
            FailedAttempts = 0,
            UpdatedAt = dateTimeProvider.UtcNow
        };

        dbContext.DriverAuthCredentials.Add(credentials);

        await dbContext.SaveChangesAsync(cancellationToken);

        return credentials.Id;
    }
}
