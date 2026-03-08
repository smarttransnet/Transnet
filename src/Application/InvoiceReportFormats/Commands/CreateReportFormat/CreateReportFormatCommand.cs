#pragma warning disable CA1054
using Application.Abstractions.Messaging;

namespace Application.InvoiceReportFormats.Commands.CreateReportFormat;

public sealed record CreateReportFormatCommand(
    string Name,
    string? Description,
    bool ShowShippingAddress,
    bool ShowTaxBreakdown,
    bool ShowTripDetails,
    string ColumnConfiguration,
    string? HeaderLogoUrl,
    string? FooterText,
    bool IsDefault,
    List<ReportFormatColumnRequest> Columns
) : ICommand<Guid>;

public sealed record ReportFormatColumnRequest(
    string ColumnKey,
    string DisplayLabel,
    decimal? WidthPercent,
    bool IsVisible,
    int SortOrder,
    string? FormatPattern
);
