using FluentValidation;

namespace Application.WorkOrders.UpdateWorkOrderStatus;

internal sealed class UpdateWorkOrderStatusCommandValidator : AbstractValidator<UpdateWorkOrderStatusCommand>
{
    public UpdateWorkOrderStatusCommandValidator()
    {
        RuleFor(c => c.WorkOrderId).NotEmpty();
        RuleFor(c => c.Status).IsInEnum();
        RuleFor(c => c.ChangedByUserId).NotEmpty();
        RuleFor(c => c.Notes).MaximumLength(1000);
    }
}
