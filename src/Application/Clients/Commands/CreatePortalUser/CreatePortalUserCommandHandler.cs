using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Clients;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Clients.Commands.CreatePortalUser;

internal sealed class CreatePortalUserCommandHandler(
    IApplicationDbContext dbContext)
    : ICommandHandler<CreatePortalUserCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreatePortalUserCommand request, CancellationToken cancellationToken)
    {
        bool emailExists = await dbContext.ClientPortalUsers
            .AnyAsync(u => u.Email == request.Email, cancellationToken);

        if (emailExists)
        {
            return Result.Failure<Guid>(Error.Conflict("ClientPortalUser.EmailInUse", "The specified email is already registered."));
        }

        bool clientExists = await dbContext.Clients
            .AnyAsync(c => c.Id == request.ClientId, cancellationToken);

        if (!clientExists)
        {
            return Result.Failure<Guid>(Error.NotFound("Client.NotFound", "The specified client was not found."));
        }

        // Just scaffolding hashing - actual implementation would use an IPasswordHasher
        string salt = Guid.NewGuid().ToString("N");
        string hash = request.PlainTextPassword; // Replace with proper hashing referencing salt

        var user = new ClientPortalUser
        {
            Id = Guid.NewGuid(),
            ClientId = request.ClientId,
            FullName = request.FullName,
            Email = request.Email,
            PasswordSalt = salt,
            PasswordHash = hash,
            Role = request.Role,
            IsActive = true
        };

        dbContext.ClientPortalUsers.Add(user);
        await dbContext.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}
