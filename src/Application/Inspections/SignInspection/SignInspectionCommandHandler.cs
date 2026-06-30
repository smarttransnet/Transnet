using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Inspections;
using SharedKernel;

namespace Application.Inspections.SignInspection;

internal sealed class SignInspectionCommandHandler(IApplicationDbContext context)
    : ICommandHandler<SignInspectionCommand>
{
    public async Task<Result> Handle(
        SignInspectionCommand command,
        CancellationToken cancellationToken)
    {
        var inspection = await context.VehicleInspections
            .FindAsync([command.InspectionId], cancellationToken);

        if (inspection is null)
        {
            return Result.Failure(Error.NotFound(
                "Inspection.NotFound",
                $"The inspection with the ID '{command.InspectionId}' was not found."));
        }

        inspection.DriverSignature = command.SignatureData;
        inspection.DriverSignedAt = command.SignedAt;
        inspection.Status = Domain.Inspections.Enums.InspectionStatus.Submitted;

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
