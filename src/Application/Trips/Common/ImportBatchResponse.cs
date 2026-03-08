using Domain.Trips.Enums;

namespace Application.Trips.Common;

public sealed record ImportBatchResponse(
    Guid Id,
    string FileName,
    Guid UploadedByUserId,
    DateTime UploadedAt,
    int TotalRows,
    int SuccessCount,
    int FailureCount,
    ImportStatus Status,
    string? ErrorSummary);
