#pragma warning disable CA1304, CA1311, CA1862
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Extensions;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.TripCategories.GetTripCategories;

public sealed record GetTripCategoriesQuery(
    string? SearchTerm,
    bool? IsActive,
    int Page = 1,
    int PageSize = 10,
    string? SortBy = null,
    string? SortOrder = "asc"
) : IQuery<PagedList<TripCategoryMaterialResponse>>;

internal sealed class GetTripCategoriesQueryHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetTripCategoriesQuery, PagedList<TripCategoryMaterialResponse>>
{
    public async Task<Result<PagedList<TripCategoryMaterialResponse>>> Handle(
        GetTripCategoriesQuery request,
        CancellationToken cancellationToken
    ) {
        var query = dbContext.TripCategoryMaterials
            .Include(cm => cm.TripCategory)
            .Include(cm => cm.Uom)
            .AsNoTracking();

        // 1. Filter by Active status
        if (request.IsActive.HasValue)
        {
            query = query.Where(cm => cm.IsActive == request.IsActive.Value);
        }

        // 2. Filter by SearchTerm (Category, UOM Code)
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var term = request.SearchTerm.Trim().ToLower();
            query = query.Where(cm =>
                cm.TripCategory!.CategoryName.ToLower().Contains(term) ||
                cm.Uom!.UOMCode.ToLower().Contains(term)
            );
        }

        // 3. Sorting
        string sortBy = request.SortBy?.ToLower() ?? "category";
        bool isDesc = request.SortOrder?.ToLower() == "desc";

        query = sortBy switch
        {
            "category" => isDesc ? query.OrderByDescending(cm => cm.TripCategory!.CategoryName) : query.OrderBy(cm => cm.TripCategory!.CategoryName),
            "uom" => isDesc ? query.OrderByDescending(cm => cm.Uom!.UOMCode) : query.OrderBy(cm => cm.Uom!.UOMCode),
            "status" => isDesc ? query.OrderByDescending(cm => cm.IsActive) : query.OrderBy(cm => cm.IsActive),
            _ => query.OrderBy(cm => cm.TripCategory!.CategoryName)
        };

        // 4. Map to response
        var responseQuery = query.Select(cm => new TripCategoryMaterialResponse(
            cm.Id,
            cm.TripCategoryId,
            cm.TripCategory!.CategoryName,
            cm.UOMId,
            cm.Uom!.UOMCode,
            cm.IsActive
        ));

        // 5. Paginate using ToPagedListAsync extension
        var pagedList = await responseQuery.ToPagedListAsync(
            request.Page,
            request.PageSize,
            cancellationToken
        );

        return pagedList;
    }
}
