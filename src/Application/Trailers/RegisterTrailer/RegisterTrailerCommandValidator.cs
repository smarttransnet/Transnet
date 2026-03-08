using FluentValidation;

namespace Application.Trailers.RegisterTrailer;

internal sealed class RegisterTrailerCommandValidator : AbstractValidator<RegisterTrailerCommand>
{
    public RegisterTrailerCommandValidator()
    {
        RuleFor(c => c.TrailerNumber).NotEmpty().MaximumLength(50);
        RuleFor(c => c.TrailerType).NotEmpty().MaximumLength(100);
        RuleFor(c => c.Capacity).GreaterThan(0);
        RuleFor(c => c.CapacityUnit).NotEmpty().MaximumLength(20);
        RuleFor(c => c.Status).IsInEnum();
    }
}
