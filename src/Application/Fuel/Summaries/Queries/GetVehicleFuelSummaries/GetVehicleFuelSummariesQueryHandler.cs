using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Fuel.Summaries.Queries.GetVehicleFuelSummaries;

internal sealed class GetVehicleFuelSummariesQueryHandler(
    IApplicationDbContext dbContext)
    : IQueryHandler<GetVehicleFuelSummariesQuery, IReadOnlyList<VehicleFuelSummaryResponse>>
{
    public async Task<Result<IReadOnlyList<VehicleFuelSummaryResponse>>> Handle(GetVehicleFuelSummariesQuery request, CancellationToken cancellationToken)
    {
        IQueryable<Domain.Fuel.VehicleFuelSummary> query = dbContext.VehicleFuelSummaries.AsQueryable();

        if (request.VehicleId.HasValue)
        {
            query = query.Where(s => s.VehicleId == request.VehicleId.Value);
        }

        if (request.PeriodMonth.HasValue)
        {
            query = query.Where(s => s.PeriodMonth == request.PeriodMonth.Value);
        }

        if (request.PeriodYear.HasValue)
        {
            query = query.Where(s => s.PeriodYear == request.PeriodYear.Value);
        }

        List<VehicleFuelSummaryResponse> summaries = await (from s in query
                                                            join v in dbContext.Vehicles on s.VehicleId equals v.Id
                                                            orderby s.PeriodYear descending, s.PeriodMonth descending
                                                            select new VehicleFuelSummaryResponse(
                                                                s.Id,
                                                                s.VehicleId,
                                                                v.PlateNumber,
                                                                s.PeriodMonth,
                                                                s.PeriodYear,
                                                                s.TotalLitres,
                                                                s.TotalCostQAR,
                                                                s.AverageCostPerLitreQAR,
                                                                s.AverageFuelEfficiencyKmPerL,
                                                                s.WoqoodTransactionCount,
                                                                s.DriverEntryCount,
                                                                s.LastUpdatedAt
                                                            ))
            .ToListAsync(cancellationToken);

        return summaries;
    }
}
