using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Fuel.Woqood.Queries.GetWoqoodImportBatch;

internal sealed class GetWoqoodImportBatchQueryHandler(
    IApplicationDbContext dbContext)
    : IQueryHandler<GetWoqoodImportBatchQuery, WoqoodImportBatchResponse>
{
    public async Task<Result<WoqoodImportBatchResponse>> Handle(GetWoqoodImportBatchQuery request, CancellationToken cancellationToken)
    {
        WoqoodImportBatchResponse? batch = await dbContext.WoqoodImportBatches
            .Where(b => b.Id == request.BatchId)
            .Select(b => new WoqoodImportBatchResponse(
                b.Id,
                b.FileName,
                b.UploadedAt,
                b.Status.ToString(),
                b.TotalRows,
                b.SuccessCount,
                b.FailureCount,
                b.TotalLitres,
                b.TotalAmountQAR
            ))
            .FirstOrDefaultAsync(cancellationToken);

        if (batch is null)
        {
            return Result.Failure<WoqoodImportBatchResponse>(Error.NotFound("WoqoodImportBatch.NotFound", "The requested import batch was not found."));
        }

        return batch;
    }
}
