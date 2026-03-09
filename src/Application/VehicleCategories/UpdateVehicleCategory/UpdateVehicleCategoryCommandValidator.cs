using FluentValidation;

namespace Application.VehicleCategories.UpdateVehicleCategory;

public sealed class UpdateVehicleCategoryCommandValidator : AbstractValidator<UpdateVehicleCategoryCommand>
{
    public UpdateVehicleCategoryCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        RuleFor(c => c.Name).NotEmpty().MaximumLength(100);
        RuleFor(c => c.Description).MaximumLength(500);
    }
}
