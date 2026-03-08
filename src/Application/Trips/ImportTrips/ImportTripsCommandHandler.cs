using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Trips;
using Domain.Trips.Enums;
using SharedKernel;

namespace Application.Trips.ImportTrips;

internal sealed class ImportTripsCommandHandler : ICommandHandler<ImportTripsCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public ImportTripsCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Guid>> Handle(ImportTripsCommand request, CancellationToken cancellationToken)
    {
        // Skeleton implementation for Excel import
        // In a real scenario, this would involve:
        // 1. Creating an ImportBatch record with 'Processing' status
        // 2. Parsing the Excel file using a library like EPPlus or ClosedXML
        // 3. Validating each row
        // 4. Mapping rows to Trip entities
        // 5. Bulk inserting trips and updating batch status

        ImportBatch batch = new()
        {
            Id = Guid.NewGuid(),
            FileName = request.FileName,
            UploadedByUserId = request.UploadedByUserId,
            UploadedAt = DateTime.UtcNow,
            TotalRows = 0, // Would be updated after parsing
            SuccessCount = 0,
            FailureCount = 0,
            Status = ImportStatus.Processing,
            ErrorSummary = "Excel parsing logic not yet implemented."
        };

        _context.ImportBatches.Add(batch);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(batch.Id);
    }
}
