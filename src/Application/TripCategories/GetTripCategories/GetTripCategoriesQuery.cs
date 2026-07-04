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
) : IQuery<PagedList<TripCategoryResponse>>;

internal sealed class GetTripCategoriesQueryHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetTripCategoriesQuery, PagedList<TripCategoryResponse>>
{
    public async Task<Result<PagedList<TripCategoryResponse>>> Handle(
        GetTripCategoriesQuery request,
        CancellationToken cancellationToken
    ) {
        var query = dbContext.TripCategories
            .Include(c => c.CategoryMaterials)
                .ThenInclude(cm => cm.Uom)
            .AsNoTracking();

        // 1. Filter by Active status
        if (request.IsActive.HasValue)
        {
            query = query.Where(c => c.IsActive == request.IsActive.Value);
        }

        // 2. Filter by SearchTerm (Category, UOM Code)
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var term = request.SearchTerm.Trim().ToLower();
            query = query.Where(c =>
                c.CategoryName.ToLower().Contains(term) ||
                c.CategoryMaterials.Any(cm => cm.Uom != null && cm.Uom.UOMCode.ToLower().Contains(term))
            );
        }

        // 3. Sorting
        string sortBy = request.SortBy?.ToLower() ?? "category";
        bool isDesc = request.SortOrder?.ToLower() == "desc";

        query = sortBy switch
        {
            "category" => isDesc ? query.OrderByDescending(c => c.CategoryName) : query.OrderBy(c => c.CategoryName),
            "status" => isDesc ? query.OrderByDescending(c => c.IsActive) : query.OrderBy(c => c.IsActive),
            _ => query.OrderBy(c => c.CategoryName)
        };

        // 4. Map to response
        var responseQuery = query.Select(c => new TripCategoryResponse(
            c.Id,
            c.CategoryName,
            c.IsActive,
            c.CategoryMaterials
                .Where(cm => cm.IsActive && cm.Uom != null)
                .Select(cm => new UomDto(cm.Id, cm.UOMId, cm.Uom!.UOMCode, cm.Uom!.Description ?? string.Empty))
                .ToList()
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
