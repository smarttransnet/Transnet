using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Fuel;
using Domain.Fuel.Enums;
using SharedKernel;

namespace Application.Fuel.Woqood.Commands.ImportWoqoodBatch;

internal sealed class ImportWoqoodBatchCommandHandler(
    IApplicationDbContext dbContext)
    : ICommandHandler<ImportWoqoodBatchCommand, Guid>
{
    public async Task<Result<Guid>> Handle(ImportWoqoodBatchCommand request, CancellationToken cancellationToken)
    {
        var batch = new WoqoodImportBatch
        {
            Id = Guid.NewGuid(),
            FileName = request.FileName,
            UploadedByUserId = request.UserId,
            UploadedAt = DateTime.UtcNow,
            Status = ImportStatus.Pending
        };

        dbContext.WoqoodImportBatches.Add(batch);
        await dbContext.SaveChangesAsync(cancellationToken);

        // TODO: Enqueue background job to process the Excel file

        return batch.Id;
    }
}
