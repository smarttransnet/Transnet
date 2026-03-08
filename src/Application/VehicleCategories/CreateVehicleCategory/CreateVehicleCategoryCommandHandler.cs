using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Assets;
using SharedKernel;

namespace Application.VehicleCategories.CreateVehicleCategory;

internal sealed class CreateVehicleCategoryCommandHandler(IApplicationDbContext dbContext, IDateTimeProvider dateTimeProvider)
    : ICommandHandler<CreateVehicleCategoryCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateVehicleCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = new VehicleCategory
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            IsActive = true,
            CreatedAt = dateTimeProvider.UtcNow
        };

        dbContext.VehicleCategories.Add(category);

        await dbContext.SaveChangesAsync(cancellationToken);

        return category.Id;
    }
}
