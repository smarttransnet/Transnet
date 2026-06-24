using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.TripCategories.GetTripCategoriesLookup;

public sealed record TripCategoryLookupResponse(Guid Id, string CategoryName, bool IsActive);

public sealed record GetTripCategoriesLookupQuery(bool? IsActive = true) : IQuery<List<TripCategoryLookupResponse>>;

internal sealed class GetTripCategoriesLookupQueryHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetTripCategoriesLookupQuery, List<TripCategoryLookupResponse>>
{
    public async Task<Result<List<TripCategoryLookupResponse>>> Handle(
        GetTripCategoriesLookupQuery request,
        CancellationToken cancellationToken
    ) {
        IQueryable<Domain.Trips.TripCategory> query = dbContext.TripCategories.AsNoTracking();

        if (request.IsActive.HasValue)
        {
            query = query.Where(c => c.IsActive == request.IsActive.Value);
        }

        var categories = await query
            .OrderBy(c => c.CategoryName)
            .Select(c => new TripCategoryLookupResponse(c.Id, c.CategoryName, c.IsActive))
            .ToListAsync(cancellationToken);

        return categories;
    }
}
