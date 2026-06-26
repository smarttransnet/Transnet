using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Inspections.GetVehicleInspections;

internal sealed class GetVehicleInspectionsQueryHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetVehicleInspectionsQuery, List<VehicleInspectionResponse>>
{
    public async Task<Result<List<VehicleInspectionResponse>>> Handle(GetVehicleInspectionsQuery request, CancellationToken cancellationToken)
    {
        List<VehicleInspectionResponse> inspections = await dbContext.VehicleInspections
            .AsNoTracking()
            .Include(i => i.InspectionResults)
            .Where(i => i.VehicleId == request.VehicleId)
            .OrderByDescending(i => i.InspectedAt)
            .Take(10)
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
                    r.Status,
                    r.Remarks,
                    r.RecordedAt)).ToList()))
            .ToListAsync(cancellationToken);

        return inspections;
    }
}
