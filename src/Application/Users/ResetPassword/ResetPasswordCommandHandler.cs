using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Users.ResetPassword;

internal sealed class ResetPasswordCommandHandler(IApplicationDbContext context, IPasswordHasher passwordHasher)
    : ICommandHandler<ResetPasswordCommand>
{
    public async Task<Result> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        User? user = await context.Users.FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

        if (user is null)
        {
            return Result.Failure(UserErrors.NotFoundByEmail);
        }

        user.PasswordHash = passwordHasher.Hash(request.NewPassword);

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
