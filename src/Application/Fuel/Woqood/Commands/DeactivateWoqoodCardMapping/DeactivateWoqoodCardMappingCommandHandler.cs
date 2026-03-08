using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Fuel.Woqood.Commands.DeactivateWoqoodCardMapping;

internal sealed class DeactivateWoqoodCardMappingCommandHandler(
    IApplicationDbContext dbContext)
    : ICommandHandler<DeactivateWoqoodCardMappingCommand>
{
    public async Task<Result> Handle(DeactivateWoqoodCardMappingCommand request, CancellationToken cancellationToken)
    {
        Domain.Fuel.WoqoodCardMapping? mapping = await dbContext.WoqoodCardMappings
            .FirstOrDefaultAsync(m => m.Id == request.MappingId, cancellationToken);

        if (mapping is null)
        {
            return Result.Failure(Error.NotFound("WoqoodCardMapping.NotFound", "Card mapping not found."));
        }

        mapping.IsActive = false;

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
