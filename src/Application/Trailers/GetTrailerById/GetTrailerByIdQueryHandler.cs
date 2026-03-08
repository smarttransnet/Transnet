using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Trailers.GetTrailers;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Trailers.GetTrailerById;

internal sealed class GetTrailerByIdQueryHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetTrailerByIdQuery, TrailerResponse>
{
    public async Task<Result<TrailerResponse>> Handle(GetTrailerByIdQuery request, CancellationToken cancellationToken)
    {
        TrailerResponse? trailer = await dbContext.Trailers
            .AsNoTracking()
            .Where(t => t.Id == request.TrailerId)
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
            .SingleOrDefaultAsync(cancellationToken);

        if (trailer is null)
        {
            return Result.Failure<TrailerResponse>(TrailerErrors.NotFound(request.TrailerId));
        }

        return trailer;
    }
}
