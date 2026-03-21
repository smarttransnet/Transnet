using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Clients;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Clients.DeleteClient;

internal sealed class DeleteClientCommandHandler(IApplicationDbContext context)
    : ICommandHandler<DeleteClientCommand>
{
    public async Task<Result> Handle(DeleteClientCommand command, CancellationToken cancellationToken)
    {
        Client? client = await context.Clients
            .FirstOrDefaultAsync(c => c.Id == command.Id, cancellationToken);

        if (client is null)
        {
            return Result.Failure(Error.NotFound("Clients.NotFound", $"The client with the Id = '{command.Id}' was not found"));
        }

        context.Clients.Remove(client);

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
