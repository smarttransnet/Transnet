using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Drivers;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Drivers.Auth.Logout;

internal sealed class LogoutCommandHandler(IApplicationDbContext dbContext) : ICommandHandler<LogoutCommand>
{
    public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        DriverAuthCredential? credential = await dbContext.DriverAuthCredentials
            .FirstOrDefaultAsync(c => c.DriverId == request.DriverId, cancellationToken);

        if (credential is null)
        {
            return Result.Success();
        }

        credential.RefreshToken = null;
        credential.RefreshTokenExpiresAt = null;
        credential.DeviceToken = null;

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
