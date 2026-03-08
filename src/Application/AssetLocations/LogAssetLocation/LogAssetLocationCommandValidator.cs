using FluentValidation;

namespace Application.AssetLocations.LogAssetLocation;

internal sealed class LogAssetLocationCommandValidator : AbstractValidator<LogAssetLocationCommand>
{
    public LogAssetLocationCommandValidator()
    {
        RuleFor(c => c.AssetType).IsInEnum();
        RuleFor(c => c.AssetId).NotEmpty();
        RuleFor(c => c.Latitude).InclusiveBetween(-90, 90);
        RuleFor(c => c.Longitude).InclusiveBetween(-180, 180);
        RuleFor(c => c.LocationName).MaximumLength(200);
        RuleFor(c => c.Source).IsInEnum();
    }
}
