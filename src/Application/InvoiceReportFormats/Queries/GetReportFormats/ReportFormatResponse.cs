namespace Application.InvoiceReportFormats.Queries.GetReportFormats;

public sealed record ReportFormatResponse(
    Guid Id,
    string Name,
    string? Description,
    bool ShowShippingAddress,
    bool ShowTaxBreakdown,
    bool ShowTripDetails,
    string ColumnConfiguration,
    bool IsDefault,
    bool IsActive
);
