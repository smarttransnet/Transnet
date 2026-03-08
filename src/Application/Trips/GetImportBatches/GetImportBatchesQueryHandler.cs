using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Trips.Common;
using Domain.Trips;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Trips.GetImportBatches;

internal sealed class GetImportBatchesQueryHandler : IQueryHandler<GetImportBatchesQuery, List<ImportBatchResponse>>
{
    private readonly IApplicationDbContext _context;

    public GetImportBatchesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<ImportBatchResponse>>> Handle(GetImportBatchesQuery request, CancellationToken cancellationToken)
    {
        List<ImportBatchResponse> batches = await _context.ImportBatches
            .OrderByDescending(b => b.UploadedAt)
            .Select(b => new ImportBatchResponse(
                b.Id,
                b.FileName,
                b.UploadedByUserId,
                b.UploadedAt,
                b.TotalRows,
                b.SuccessCount,
                b.FailureCount,
                b.Status,
                b.ErrorSummary))
            .ToListAsync(cancellationToken);

        return Result.Success(batches);
    }
}
