using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Assets;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.VehicleCategories.UpdateVehicleCategory;

internal sealed class UpdateVehicleCategoryCommandHandler(IApplicationDbContext dbContext)
    : ICommandHandler<UpdateVehicleCategoryCommand>
{
    public async Task<Result> Handle(UpdateVehicleCategoryCommand request, CancellationToken cancellationToken)
    {
        VehicleCategory? category = await dbContext.VehicleCategories
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (category is null)
        {
            return Result.Failure(Error.NotFound("VehicleCategory.NotFound", "The vehicle category was not found."));
        }

        category.Name = request.Name;
        category.Description = request.Description;
        category.IsActive = request.IsActive;

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
