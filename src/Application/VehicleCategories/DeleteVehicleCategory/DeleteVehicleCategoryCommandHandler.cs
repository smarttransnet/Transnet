using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using Domain.Assets;

namespace Application.VehicleCategories.DeleteVehicleCategory;

internal sealed class DeleteVehicleCategoryCommandHandler(IApplicationDbContext dbContext)
    : ICommandHandler<DeleteVehicleCategoryCommand>
{
    public async Task<Result> Handle(DeleteVehicleCategoryCommand request, CancellationToken cancellationToken)
    {
        VehicleCategory? category = await dbContext.VehicleCategories
            .FirstOrDefaultAsync(v => v.Id == request.Id, cancellationToken);

        if (category is null)
        {
            return Result.Failure(Error.NotFound("VehicleCategory.NotFound", "The vehicle category was not found."));
        }

        dbContext.VehicleCategories.Remove(category);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
