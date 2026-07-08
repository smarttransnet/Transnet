using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Reports.GetMaintenanceCosts;

internal sealed class GetMaintenanceCostsQueryHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetMaintenanceCostsQuery, List<MaintenanceCostResponse>>
{
    public async Task<Result<List<MaintenanceCostResponse>>> Handle(GetMaintenanceCostsQuery request, CancellationToken cancellationToken)
    {
        List<MaintenanceCostResponse> costs = await dbContext.WorkOrders
            .AsNoTracking()
            .Where(wo => wo.CompletedAt >= request.StartDate && wo.CompletedAt <= request.EndDate)
            .GroupBy(wo => wo.Vehicle)
            .Select(g => new MaintenanceCostResponse(
                g.Key.Id,
                g.Key.ChassisNumber,
                g.Count(),
                g.Sum(wo => wo.WorkOrderItems.Sum(i => i.TotalCostQAR))
            ))
            .ToListAsync(cancellationToken);

        return costs;
    }
}
