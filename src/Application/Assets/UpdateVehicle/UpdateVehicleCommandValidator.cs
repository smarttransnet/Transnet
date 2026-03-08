using FluentValidation;

namespace Application.Assets.UpdateVehicle;

internal sealed class UpdateVehicleCommandValidator : AbstractValidator<UpdateVehicleCommand>
{
    public UpdateVehicleCommandValidator()
    {
        RuleFor(c => c.VehicleId).NotEmpty();
        RuleFor(c => c.RegistrationNumber).NotEmpty().MaximumLength(50);
        RuleFor(c => c.PlateNumber).NotEmpty().MaximumLength(50);
        RuleFor(c => c.Make).NotEmpty().MaximumLength(100);
        RuleFor(c => c.Model).NotEmpty().MaximumLength(100);
        RuleFor(c => c.Year).GreaterThan(1900);
        RuleFor(c => c.VehicleCategoryId).NotEmpty();
        RuleFor(c => c.VehicleType).IsInEnum();
        RuleFor(c => c.Status).IsInEnum();
        RuleFor(c => c.OdometerReading).GreaterThanOrEqualTo(0);
    }
}
