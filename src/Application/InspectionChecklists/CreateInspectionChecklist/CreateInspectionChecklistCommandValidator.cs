using FluentValidation;

namespace Application.InspectionChecklists.CreateInspectionChecklist;

internal sealed class CreateInspectionChecklistCommandValidator : AbstractValidator<CreateInspectionChecklistCommand>
{
    public CreateInspectionChecklistCommandValidator()
    {
        RuleFor(c => c.Name).NotEmpty().MaximumLength(200);
    }
}
