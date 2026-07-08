using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Inspections.GetInspections;

internal sealed class GetInspectionsQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetInspectionsQuery, List<InspectionResponse>>
{
    public async Task<Result<List<InspectionResponse>>> Handle(
        GetInspectionsQuery query,
        CancellationToken cancellationToken)
    {
        var inspectionsQuery = context.VehicleInspections
            .Include(i => i.Vehicle)
            .Include(i => i.InspectionChecklist)
            .AsNoTracking();

        if (query.VehicleId.HasValue)
        {
            inspectionsQuery = inspectionsQuery.Where(i => i.VehicleId == query.VehicleId.Value);
        }

        var inspections = await inspectionsQuery
            .OrderByDescending(i => i.InspectedAt)
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(i => new InspectionResponse(
                i.Id,
                i.VehicleId,
                i.Vehicle.ChassisNumber,
                i.InspectionChecklist.Name,
                i.InspectionType,
                i.InspectedAt,
                i.Status))
            .ToListAsync(cancellationToken);

        return inspections;
    }
}
