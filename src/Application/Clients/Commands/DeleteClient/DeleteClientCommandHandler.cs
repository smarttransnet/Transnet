using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Clients;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Clients.Commands.DeleteClient;

internal sealed class DeleteClientCommandHandler(IApplicationDbContext dbContext)
    : ICommandHandler<DeleteClientCommand>
{
    public async Task<Result> Handle(DeleteClientCommand request, CancellationToken cancellationToken)
    {
        Client? client = await dbContext.Clients
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (client is null)
        {
            return Result.Failure(Error.NotFound("Client.NotFound", "The client was not found."));
        }

        dbContext.Clients.Remove(client);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
