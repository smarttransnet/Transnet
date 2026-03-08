using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Trips.Common;
using Domain.Trips;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Trips.GetImportBatchById;

internal sealed class GetImportBatchByIdQueryHandler : IQueryHandler<GetImportBatchByIdQuery, ImportBatchResponse>
{
    private readonly IApplicationDbContext _context;

    public GetImportBatchByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<ImportBatchResponse>> Handle(GetImportBatchByIdQuery request, CancellationToken cancellationToken)
    {
        ImportBatch? batch = await _context.ImportBatches
            .FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken);

        if (batch is null)
        {
            return Result.Failure<ImportBatchResponse>(Error.NotFound("ImportBatch.NotFound", $"Import batch with ID {request.Id} was not found."));
        }

        ImportBatchResponse response = new(
            batch.Id,
            batch.FileName,
            batch.UploadedByUserId,
            batch.UploadedAt,
            batch.TotalRows,
            batch.SuccessCount,
            batch.FailureCount,
            batch.Status,
            batch.ErrorSummary);

        return Result.Success(response);
    }
}
