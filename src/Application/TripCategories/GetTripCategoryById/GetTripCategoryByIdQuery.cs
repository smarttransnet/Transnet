using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.TripCategories.GetTripCategoryById;

public sealed record GetTripCategoryByIdQuery(Guid Id) : IQuery<TripCategoryMaterialResponse>;

internal sealed class GetTripCategoryByIdQueryHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetTripCategoryByIdQuery, TripCategoryMaterialResponse>
{
    public async Task<Result<TripCategoryMaterialResponse>> Handle(
        GetTripCategoryByIdQuery request,
        CancellationToken cancellationToken
    ) {
        var cm = await dbContext.TripCategoryMaterials
            .Include(cm => cm.TripCategory)
            .Include(cm => cm.Material)
            .Include(cm => cm.Uom)
            .AsNoTracking()
            .SingleOrDefaultAsync(cm => cm.Id == request.Id, cancellationToken);

        if (cm is null)
        {
            return Result.Failure<TripCategoryMaterialResponse>(Error.NotFound(
                "TripCategoryMaterial.NotFound",
                $"The trip category material mapping with ID '{request.Id}' was not found."
            ));
        }

        return new TripCategoryMaterialResponse(
            cm.Id,
            cm.TripCategoryId,
            cm.TripCategory!.CategoryName,
            cm.MaterialId,
            cm.Material!.MaterialName,
            cm.UOMId,
            cm.Uom!.UOMCode,
            cm.IsActive
        );
    }
}
