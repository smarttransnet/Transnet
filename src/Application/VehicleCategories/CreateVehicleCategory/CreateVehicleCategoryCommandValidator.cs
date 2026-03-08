using FluentValidation;

namespace Application.VehicleCategories.CreateVehicleCategory;

internal sealed class CreateVehicleCategoryCommandValidator : AbstractValidator<CreateVehicleCategoryCommand>
{
    public CreateVehicleCategoryCommandValidator()
    {
        RuleFor(c => c.Name).NotEmpty().MaximumLength(100);
        RuleFor(c => c.Description).MaximumLength(500);
    }
}
