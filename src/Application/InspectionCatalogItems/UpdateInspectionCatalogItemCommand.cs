using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.InspectionCatalogItems.UpdateInspectionCatalogItem;

public sealed record UpdateInspectionCatalogItemCommand(
    Guid Id,
    string Category,
    string ItemName,
    int SortOrder,
    bool IsActive) : ICommand;

internal sealed class UpdateInspectionCatalogItemCommandHandler(IApplicationDbContext dbContext)
    : ICommandHandler<UpdateInspectionCatalogItemCommand>
{
    public async Task<Result> Handle(UpdateInspectionCatalogItemCommand request, CancellationToken cancellationToken)
    {
        var item = await dbContext.InspectionCatalogItems
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (item is null)
        {
            return Result.Failure(Error.NotFound("InspectionCatalogItem.NotFound", "The catalog item was not found."));
        }

        item.Category = request.Category;
        item.ItemName = request.ItemName;
        item.SortOrder = request.SortOrder;
        item.IsActive = request.IsActive;

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
