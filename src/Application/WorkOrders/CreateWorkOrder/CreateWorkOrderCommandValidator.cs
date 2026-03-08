using FluentValidation;

namespace Application.WorkOrders.CreateWorkOrder;

internal sealed class CreateWorkOrderCommandValidator : AbstractValidator<CreateWorkOrderCommand>
{
    public CreateWorkOrderCommandValidator()
    {
        RuleFor(c => c.Title).NotEmpty().MaximumLength(200);
        RuleFor(c => c.Description).NotEmpty().MaximumLength(1000);
        RuleFor(c => c.VehicleId).NotEmpty();
        RuleFor(c => c.Priority).IsInEnum();
    }
}
