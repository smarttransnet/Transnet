using SharedKernel;

namespace Domain.Billing;

public sealed class InvoiceReportFormat : Entity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? TemplateFileUrl { get; set; }
    public bool ShowShippingAddress { get; set; }
    public bool ShowTaxBreakdown { get; set; }
    public bool ShowTripDetails { get; set; }
    public string ColumnConfiguration { get; set; } = string.Empty;
    public string? HeaderLogoUrl { get; set; }
    public string? FooterText { get; set; }
    public bool IsDefault { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation Properties
    public ICollection<Invoice> Invoices { get; set; } = [];
    public ICollection<ReportFormatColumn> ReportColumns { get; set; } = [];
}
