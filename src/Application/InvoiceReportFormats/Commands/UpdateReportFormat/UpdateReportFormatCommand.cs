#pragma warning disable CA1054
using Application.Abstractions.Messaging;

namespace Application.InvoiceReportFormats.Commands.UpdateReportFormat;

// Reusing ReportFormatColumnRequest from Create namespace
public sealed record UpdateReportFormatCommand(
    Guid ReportFormatId,
    string Name,
    string? Description,
    bool ShowShippingAddress,
    bool ShowTaxBreakdown,
    bool ShowTripDetails,
    string ColumnConfiguration,
    string? HeaderLogoUrl,
    string? FooterText,
    bool IsDefault,
    bool IsActive,
    List<Application.InvoiceReportFormats.Commands.CreateReportFormat.ReportFormatColumnRequest> Columns
) : ICommand;
