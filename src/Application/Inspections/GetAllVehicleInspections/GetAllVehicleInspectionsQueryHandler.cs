using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Inspections.GetAllVehicleInspections;

internal sealed class GetAllVehicleInspectionsQueryHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetAllVehicleInspectionsQuery, List<VehicleInspectionResponse>>
{
    public async Task<Result<List<VehicleInspectionResponse>>> Handle(GetAllVehicleInspectionsQuery request, CancellationToken cancellationToken)
    {
        var inspections = await dbContext.VehicleInspections
            .AsNoTracking()
            .OrderByDescending(i => i.InspectedAt)
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
                new List<InspectionResultResponse>())) // Don't include results in list view for performance
            .ToListAsync(cancellationToken);

        return inspections;
    }
}
