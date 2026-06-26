using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.InspectionCatalogItems.GetInspectionCatalogItems;

public sealed record InspectionCatalogItemResponse(
    Guid Id,
    string Category,
    string ItemName,
    int SortOrder,
    bool IsActive);

public sealed record GetInspectionCatalogItemsQuery : IQuery<List<InspectionCatalogItemResponse>>;

internal sealed class GetInspectionCatalogItemsQueryHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetInspectionCatalogItemsQuery, List<InspectionCatalogItemResponse>>
{
    public async Task<Result<List<InspectionCatalogItemResponse>>> Handle(GetInspectionCatalogItemsQuery request, CancellationToken cancellationToken)
    {
        var items = await dbContext.InspectionCatalogItems
            .OrderBy(i => i.Category)
            .ThenBy(i => i.SortOrder)
            .Select(i => new InspectionCatalogItemResponse(
                i.Id,
                i.Category,
                i.ItemName,
                i.SortOrder,
                i.IsActive))
            .ToListAsync(cancellationToken);

        return items;
    }
}
