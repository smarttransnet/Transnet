using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.TripCategories.GetMaterialsLookup;

public sealed record MaterialLookupResponse(Guid Id, Guid TripCategoryId, string MaterialName, bool IsActive);

public sealed record GetMaterialsLookupQuery(Guid? TripCategoryId = null, bool? IsActive = true) : IQuery<List<MaterialLookupResponse>>;

internal sealed class GetMaterialsLookupQueryHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetMaterialsLookupQuery, List<MaterialLookupResponse>>
{
    public async Task<Result<List<MaterialLookupResponse>>> Handle(
        GetMaterialsLookupQuery request,
        CancellationToken cancellationToken
    ) {
        IQueryable<Domain.Trips.Material> query = dbContext.Materials.AsNoTracking();

        if (request.TripCategoryId.HasValue)
        {
            query = query.Where(m => m.TripCategoryId == request.TripCategoryId.Value);
        }

        if (request.IsActive.HasValue)
        {
            query = query.Where(m => m.IsActive == request.IsActive.Value);
        }

        var materials = await query
            .OrderBy(m => m.MaterialName)
            .Select(m => new MaterialLookupResponse(m.Id, m.TripCategoryId, m.MaterialName, m.IsActive))
            .ToListAsync(cancellationToken);

        return materials;
    }
}
