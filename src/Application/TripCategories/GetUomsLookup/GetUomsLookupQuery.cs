using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.TripCategories.GetUomsLookup;

public sealed record UomLookupResponse(Guid Id, string UomCode, string? Description, bool IsActive);

public sealed record GetUomsLookupQuery(bool? IsActive = true) : IQuery<List<UomLookupResponse>>;

internal sealed class GetUomsLookupQueryHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetUomsLookupQuery, List<UomLookupResponse>>
{
    public async Task<Result<List<UomLookupResponse>>> Handle(
        GetUomsLookupQuery request,
        CancellationToken cancellationToken
    ) {
        IQueryable<Domain.Trips.Uom> query = dbContext.Uoms.AsNoTracking();

        if (request.IsActive.HasValue)
        {
            query = query.Where(u => u.IsActive == request.IsActive.Value);
        }

        var uoms = await query
            .OrderBy(u => u.UOMCode)
            .Select(u => new UomLookupResponse(u.Id, u.UOMCode, u.Description, u.IsActive))
            .ToListAsync(cancellationToken);

        return uoms;
    }
}
