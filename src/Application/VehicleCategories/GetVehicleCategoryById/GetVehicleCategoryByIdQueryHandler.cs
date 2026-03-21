using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.VehicleCategories.GetVehicleCategoryById;

internal sealed class GetVehicleCategoryByIdQueryHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetVehicleCategoryByIdQuery, VehicleCategoryResponse>
{
    public async Task<Result<VehicleCategoryResponse>> Handle(GetVehicleCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await dbContext.VehicleCategories
            .AsNoTracking()
            .Where(c => c.Id == request.Id)
            .Select(c => new VehicleCategoryResponse(c.Id, c.Name, c.Description, c.IsActive))
            .FirstOrDefaultAsync(cancellationToken);

        if (category is null)
        {
            return Result.Failure<VehicleCategoryResponse>(Error.NotFound("VehicleCategory.NotFound", $"The vehicle category with ID {request.Id} was not found."));
        }

        return category;
    }
}
