using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Clients;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.ClientPortalAuth.Commands.Refresh;

internal sealed class RefreshCommandHandler(
    IApplicationDbContext dbContext,
    ITokenProvider tokenProvider)
    : ICommandHandler<RefreshCommand, RefreshResponse>
{
    public async Task<Result<RefreshResponse>> Handle(RefreshCommand request, CancellationToken cancellationToken)
    {
        ClientPortalUser? user = await dbContext.ClientPortalUsers
            .FirstOrDefaultAsync(u => u.Id == request.UserId && u.ClientId == request.ClientId, cancellationToken);

        if (user is null || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiresAt < DateTime.UtcNow)
        {
            return Result.Failure<RefreshResponse>(Error.Problem("Auth.InvalidToken", "Invalid or expired refresh token."));
        }

        if (!user.IsActive)
        {
            return Result.Failure<RefreshResponse>(Error.Problem("Auth.Inactive", "Account is inactive."));
        }

        string accessToken = tokenProvider.CreateForClientPortalUser(user);
        string newRefreshToken = tokenProvider.CreateRefreshToken();

        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(7);

        await dbContext.SaveChangesAsync(cancellationToken);

        return new RefreshResponse(accessToken, newRefreshToken);
    }
}
