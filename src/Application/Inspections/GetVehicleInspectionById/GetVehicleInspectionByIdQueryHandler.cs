using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Inspections.GetVehicleInspectionById;

internal sealed class GetVehicleInspectionByIdQueryHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetVehicleInspectionByIdQuery, VehicleInspectionResponse>
{
    public async Task<Result<VehicleInspectionResponse>> Handle(GetVehicleInspectionByIdQuery request, CancellationToken cancellationToken)
    {
        var inspection = await dbContext.VehicleInspections
            .AsNoTracking()
            .Include(i => i.InspectionResults)
            .Where(i => i.Id == request.Id)
            .Select(i => new VehicleInspectionResponse(
                i.Id,
                i.VehicleId,
                i.InspectionChecklistId,
                i.InspectionType,
                i.DriverId,
                i.TripId,
                i.InspectedAt,
                i.DriverSignature,
                i.DriverSignedAt,
                i.Notes,
                i.OdometerReading,
                i.Status,
                i.InspectionResults.Select(r => new InspectionResultResponse(
                    r.ChecklistItemId,
                    r.IsPassed,
                    r.Remarks,
                    r.RecordedAt)).ToList()))
            .FirstOrDefaultAsync(cancellationToken);

        if (inspection is null)
        {
            return Result.Failure<VehicleInspectionResponse>(Error.NotFound("Inspections.NotFound", $"The inspection with ID {request.Id} was not found."));
        }

        return inspection;
    }
}
