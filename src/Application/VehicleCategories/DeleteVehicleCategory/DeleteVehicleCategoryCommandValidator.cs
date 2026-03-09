using FluentValidation;

namespace Application.VehicleCategories.DeleteVehicleCategory;

public sealed class DeleteVehicleCategoryCommandValidator : AbstractValidator<DeleteVehicleCategoryCommand>
{
    public DeleteVehicleCategoryCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
    }
}
