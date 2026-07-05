#pragma warning disable CA1304, CA1311, CA1862, IDE0045
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.VehicleCategoryMappings.GetMappings;

public sealed record GetVehicleCategoryMappingsQuery(
    string? SearchTerm,
    bool? IsActive,
    int Page,
    int PageSize,
    string? SortBy,
    string? SortOrder
) : IQuery<PagedList<VehicleCategoryMappingResponse>>;

internal sealed class GetVehicleCategoryMappingsQueryHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetVehicleCategoryMappingsQuery, PagedList<VehicleCategoryMappingResponse>>
{
    public async Task<Result<PagedList<VehicleCategoryMappingResponse>>> Handle(
        GetVehicleCategoryMappingsQuery request,
        CancellationToken cancellationToken
    ) {
        var query = dbContext.VehicleCategories
            .Include(c => c.VehicleCategoryUoms)
            .ThenInclude(m => m.Uom)
            .AsNoTracking();

        if (request.IsActive.HasValue)
        {
            query = query.Where(c => c.IsActive == request.IsActive.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var searchTerm = request.SearchTerm.ToLower();
            query = query.Where(c => c.Name.ToLower().Contains(searchTerm));
        }

        // We map to response and then we'll do pagination (if needed).
        // Since VehicleCategory and VehicleCategoryUoms is a one-to-many, 
        // we can filter the nested list or just return them.
        
        var totalCount = await query.CountAsync(cancellationToken);
        
        var isDescending = request.SortOrder?.ToLower() == "desc";
        
        if (request.SortBy?.ToLower() == "name")
        {
            query = isDescending ? query.OrderByDescending(c => c.Name) : query.OrderBy(c => c.Name);
        }
        else
        {
            query = isDescending ? query.OrderByDescending(c => c.CreatedAt) : query.OrderBy(c => c.CreatedAt);
        }

        var categories = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(c => new VehicleCategoryMappingResponse(
                c.Id,
                c.Name,
                c.IsActive,
                c.VehicleCategoryUoms
                    .Where(m => m.IsActive)
                    .Select(m => new VehicleCategoryUomResponse(
                        m.Id,
                        m.UOMId,
                        m.Uom!.UOMCode,
                        m.Uom.Description
                    )).ToList()
            ))
            .ToListAsync(cancellationToken);

        return new PagedList<VehicleCategoryMappingResponse>(
            categories,
            request.Page,
            request.PageSize,
            totalCount
        );
    }
}
