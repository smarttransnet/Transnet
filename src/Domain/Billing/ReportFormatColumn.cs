using SharedKernel;

namespace Domain.Billing;

public sealed class ReportFormatColumn : Entity
{
    public Guid Id { get; set; }
    public Guid InvoiceReportFormatId { get; set; }
    public string ColumnKey { get; set; } = string.Empty;
    public string DisplayLabel { get; set; } = string.Empty;
    public decimal? WidthPercent { get; set; }
    public bool IsVisible { get; set; } = true;
    public int SortOrder { get; set; }
    public string? FormatPattern { get; set; }

    // Navigation Properties
    public InvoiceReportFormat? ReportFormat { get; set; }
}
