using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Fuel;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Fuel.Woqood.Commands.DeleteWoqoodCardMapping;

internal sealed class DeleteWoqoodCardMappingCommandHandler(IApplicationDbContext context)
    : ICommandHandler<DeleteWoqoodCardMappingCommand>
{
    public async Task<Result> Handle(DeleteWoqoodCardMappingCommand command, CancellationToken cancellationToken)
    {
        WoqoodCardMapping? mapping = await context.WoqoodCardMappings
            .FirstOrDefaultAsync(m => m.Id == command.Id, cancellationToken);

        if (mapping is null)
        {
            return Result.Failure(Error.NotFound("WoqoodCardMappings.NotFound", $"The Woqood card mapping with the Id = '{command.Id}' was not found"));
        }

        context.WoqoodCardMappings.Remove(mapping);

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
