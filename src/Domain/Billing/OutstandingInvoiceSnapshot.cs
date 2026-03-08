using Domain.Billing.Enums;
using SharedKernel;

namespace Domain.Billing;

public sealed class OutstandingInvoiceSnapshot : Entity
{
    public Guid Id { get; set; }
    public Guid OutstandingInvoiceReportId { get; set; }
    public Guid InvoiceId { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public DateTime IssuedAt { get; set; }
    public DateOnly DueDate { get; set; }
    public decimal OriginalAmountQAR { get; set; }
    public decimal OutstandingAmountQAR { get; set; }
    public int AgingDays { get; set; }
    public AgingBucket AgingBucket { get; set; }

    // Navigation Properties
    public OutstandingInvoiceReport? Report { get; set; }
    public Invoice? Invoice { get; set; }
}
