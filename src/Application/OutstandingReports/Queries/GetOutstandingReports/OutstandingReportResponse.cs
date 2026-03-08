namespace Application.OutstandingReports.Queries.GetOutstandingReports;

public sealed record OutstandingReportResponse(
    Guid Id,
    Guid ClientId,
    string ClientName,
    DateTime GeneratedAt,
    int PeriodMonth,
    int PeriodYear,
    decimal TotalOutstandingQAR,
    int InvoiceCount,
    DateOnly? OldestInvoiceDate,
    string DeliveryStatus,
    DateTime? SentAt
);
