using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using System;
using System.Threading;
using System.Threading.Tasks;

using Application.TripCategories.GetTripCategories;

namespace Application.TripCategories.GetTripCategoryById;

public sealed record GetTripCategoryByIdQuery(Guid Id) : IQuery<TripCategoryResponse>;

internal sealed class GetTripCategoryByIdQueryHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetTripCategoryByIdQuery, TripCategoryResponse>
{
    public async Task<Result<TripCategoryResponse>> Handle(
        GetTripCategoryByIdQuery request,
        CancellationToken cancellationToken
    ) {
        var c = await dbContext.TripCategories
            .Include(c => c.CategoryMaterials)
                .ThenInclude(cm => cm.Uom)
            .AsNoTracking()
            .SingleOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (c is null)
        {
            return Result.Failure<TripCategoryResponse>(Error.NotFound(
                "TripCategory.NotFound",
                $"The trip category with ID '{request.Id}' was not found."
            ));
        }

        return new TripCategoryResponse(
            c.Id,
            c.CategoryName,
            c.IsActive,
            c.CategoryMaterials
                .Where(cm => cm.IsActive && cm.Uom != null)
                .Select(cm => new UomDto(cm.Id, cm.UOMId, cm.Uom!.UOMCode, cm.Uom!.Description ?? string.Empty))
                .ToList()
        );
    }
}
