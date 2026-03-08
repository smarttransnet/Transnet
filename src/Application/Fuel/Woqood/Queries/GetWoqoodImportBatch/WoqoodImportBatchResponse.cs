namespace Application.Fuel.Woqood.Queries.GetWoqoodImportBatch;

public sealed record WoqoodImportBatchResponse(
    Guid Id,
    string FileName,
    DateTime UploadedAt,
    string Status,
    int TotalRows,
    int SuccessCount,
    int FailureCount,
    decimal TotalLitres,
    decimal TotalAmountQAR
);
