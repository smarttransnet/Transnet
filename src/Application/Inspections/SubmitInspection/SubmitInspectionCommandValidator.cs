using FluentValidation;

namespace Application.Inspections.SubmitInspection;

internal sealed class SubmitInspectionCommandValidator : AbstractValidator<SubmitInspectionCommand>
{
    public SubmitInspectionCommandValidator()
    {
        RuleFor(c => c.VehicleId).NotEmpty();
        RuleFor(c => c.InspectionChecklistId).NotEmpty();
        RuleFor(c => c.InspectionType).IsInEnum();
        RuleFor(c => c.DriverId).NotEmpty();
        RuleFor(c => c.Notes).MaximumLength(1000);
        RuleFor(c => c.OdometerReading).GreaterThanOrEqualTo(0);
        RuleFor(c => c.DriverSignature).MaximumLength(200);
    }
}
