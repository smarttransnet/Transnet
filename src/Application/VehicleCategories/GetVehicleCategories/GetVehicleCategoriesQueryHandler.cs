using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.VehicleCategories.GetVehicleCategories;

internal sealed class GetVehicleCategoriesQueryHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetVehicleCategoriesQuery, List<VehicleCategoryResponse>>
{
    public async Task<Result<List<VehicleCategoryResponse>>> Handle(GetVehicleCategoriesQuery request, CancellationToken cancellationToken)
    {
        List<VehicleCategoryResponse> categories = await dbContext.VehicleCategories
            .AsNoTracking()
            .Select(c => new VehicleCategoryResponse(c.Id, c.Name, c.Description, c.IsActive))
            .ToListAsync(cancellationToken);

        return categories;
    }
}
