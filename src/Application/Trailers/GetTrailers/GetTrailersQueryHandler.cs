using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Trailers.GetTrailers;

internal sealed class GetTrailersQueryHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetTrailersQuery, List<TrailerResponse>>
{
    public async Task<Result<List<TrailerResponse>>> Handle(GetTrailersQuery request, CancellationToken cancellationToken)
    {
        List<TrailerResponse> trailers = await dbContext.Trailers
            .AsNoTracking()
            .Select(t => new TrailerResponse(
                t.Id,
                t.TrailerNumber,
                t.TrailerType,
                t.Capacity,
                t.CapacityUnit,
                t.AttachedVehicleId,
                t.Status,
                t.TotalRevenueQAR,
                t.TotalExpensesQAR,
                t.IsActive))
            .ToListAsync(cancellationToken);

        return trailers;
    }
}
