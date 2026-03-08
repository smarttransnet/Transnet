using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Assets;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Trailers.GetTrailerPerformance;

internal sealed class GetTrailerPerformanceQueryHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetTrailerPerformanceQuery, TrailerPerformanceResponse>
{
    public async Task<Result<TrailerPerformanceResponse>> Handle(GetTrailerPerformanceQuery request, CancellationToken cancellationToken)
    {
        TrailerPerformanceResponse? performance = await dbContext.Trailers
            .AsNoTracking()
            .Where(t => t.Id == request.TrailerId)
            .Select(t => new TrailerPerformanceResponse(
                t.Id,
                t.TrailerNumber,
                t.TotalRevenueQAR,
                t.TotalExpensesQAR,
                t.TotalRevenueQAR - t.TotalExpensesQAR))
            .SingleOrDefaultAsync(cancellationToken);

        if (performance is null)
        {
            return Result.Failure<TrailerPerformanceResponse>(TrailerErrors.NotFound(request.TrailerId));
        }

        return performance;
    }
}
