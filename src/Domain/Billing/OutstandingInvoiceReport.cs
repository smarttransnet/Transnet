using Domain.Billing.Enums;
using Domain.Clients;
using SharedKernel;

namespace Domain.Billing;

public sealed class OutstandingInvoiceReport : Entity
{
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }
    public DateTime GeneratedAt { get; set; }
    public int PeriodMonth { get; set; }
    public int PeriodYear { get; set; }
    public decimal TotalOutstandingQAR { get; set; }
    public int InvoiceCount { get; set; }
    public DateOnly? OldestInvoiceDate { get; set; }
    public ReminderDeliveryStatus DeliveryStatus { get; set; }
    public DateTime? SentAt { get; set; }
    public string? SentToEmail { get; set; }
    public string? ExportedFileUrl { get; set; }

    // Navigation Properties
    public Client? Client { get; set; }
    public ICollection<OutstandingInvoiceSnapshot> Snapshots { get; set; } = [];
}
