using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.InspectionCatalogItems.DeleteInspectionCatalogItem;

public sealed record DeleteInspectionCatalogItemCommand(Guid Id) : ICommand;

internal sealed class DeleteInspectionCatalogItemCommandHandler(IApplicationDbContext dbContext)
    : ICommandHandler<DeleteInspectionCatalogItemCommand>
{
    public async Task<Result> Handle(DeleteInspectionCatalogItemCommand request, CancellationToken cancellationToken)
    {
        var item = await dbContext.InspectionCatalogItems
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (item is null)
        {
            return Result.Failure(Error.NotFound("InspectionCatalogItem.NotFound", "The catalog item was not found."));
        }

        dbContext.InspectionCatalogItems.Remove(item);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
