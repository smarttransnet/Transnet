using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Inspections;
using SharedKernel;

namespace Application.InspectionCatalogItems.CreateInspectionCatalogItem;

public sealed record CreateInspectionCatalogItemCommand(
    string Category,
    string ItemName,
    int SortOrder,
    bool IsActive) : ICommand<Guid>;

internal sealed class CreateInspectionCatalogItemCommandHandler(IApplicationDbContext dbContext)
    : ICommandHandler<CreateInspectionCatalogItemCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateInspectionCatalogItemCommand request, CancellationToken cancellationToken)
    {
        var item = new InspectionCatalogItem
        {
            Id = Guid.NewGuid(),
            Category = request.Category,
            ItemName = request.ItemName,
            SortOrder = request.SortOrder,
            IsActive = request.IsActive
        };

        dbContext.InspectionCatalogItems.Add(item);

        await dbContext.SaveChangesAsync(cancellationToken);

        return item.Id;
    }
}
