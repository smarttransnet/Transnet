using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Inspections;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Inspections.DeleteVehicleInspection;

internal sealed class DeleteVehicleInspectionCommandHandler(IApplicationDbContext context)
    : ICommandHandler<DeleteVehicleInspectionCommand>
{
    public async Task<Result> Handle(DeleteVehicleInspectionCommand command, CancellationToken cancellationToken)
    {
        VehicleInspection? inspection = await context.VehicleInspections
            .Include(i => i.InspectionResults)
            .Include(i => i.Photos)
            .FirstOrDefaultAsync(i => i.Id == command.Id, cancellationToken);

        if (inspection is null)
        {
            return Result.Failure(Error.NotFound("VehicleInspections.NotFound", $"The vehicle inspection with the Id = '{command.Id}' was not found"));
        }

        context.VehicleInspections.Remove(inspection);

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
