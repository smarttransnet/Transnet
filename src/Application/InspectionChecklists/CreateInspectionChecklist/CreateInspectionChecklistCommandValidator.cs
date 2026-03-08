using FluentValidation;

namespace Application.InspectionChecklists.CreateInspectionChecklist;

internal sealed class CreateInspectionChecklistCommandValidator : AbstractValidator<CreateInspectionChecklistCommand>
{
    public CreateInspectionChecklistCommandValidator()
    {
        RuleFor(c => c.Name).NotEmpty().MaximumLength(200);
        RuleFor(c => c.InspectionType).IsInEnum();
        RuleFor(c => c.ApplicableVehicleTypes).MaximumLength(500);
    }
}
