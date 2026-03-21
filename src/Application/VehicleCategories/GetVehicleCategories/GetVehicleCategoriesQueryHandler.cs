using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.VehicleCategories;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.VehicleCategories.GetVehicleCategories;

internal sealed class GetVehicleCategoriesQueryHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetVehicleCategoriesQuery, IReadOnlyList<VehicleCategoryResponse>>
{
    public async Task<Result<IReadOnlyList<VehicleCategoryResponse>>> Handle(GetVehicleCategoriesQuery request, CancellationToken cancellationToken)
    {
        IReadOnlyList<VehicleCategoryResponse> categories = await dbContext.VehicleCategories
            .AsNoTracking()
            .Select(c => new VehicleCategoryResponse(c.Id, c.Name, c.Description, c.IsActive))
            .ToListAsync(cancellationToken);

        return Result.Success(categories);
    }
}
