using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Clients;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Clients.Commands.UpdatePortalUser;

internal sealed class UpdatePortalUserCommandHandler(
    IApplicationDbContext dbContext)
    : ICommandHandler<UpdatePortalUserCommand>
{
    public async Task<Result> Handle(UpdatePortalUserCommand request, CancellationToken cancellationToken)
    {
        ClientPortalUser? user = await dbContext.ClientPortalUsers
            .FirstOrDefaultAsync(u => u.Id == request.UserId && u.ClientId == request.ClientId, cancellationToken);

        if (user is null)
        {
            return Result.Failure(Error.NotFound("ClientPortalUser.NotFound", "The specified portal user was not found."));
        }

        user.FullName = request.FullName;
        user.Email = request.Email;
        user.Role = request.Role;
        user.IsActive = request.IsActive;

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
