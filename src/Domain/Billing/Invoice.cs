using Domain.Billing.Enums;
using Domain.Clients;
using SharedKernel;

namespace Domain.Billing;

public sealed class Invoice : Entity
{
    public Guid Id { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public Guid ClientId { get; set; }
    public Guid? QuotationId { get; set; }
    public Guid IssuedByUserId { get; set; }
    public DateTime IssuedAt { get; set; }
    public DateOnly DueDate { get; set; }
    public InvoiceStatus Status { get; set; }
    public decimal SubTotalQAR { get; set; }
    public decimal TaxAmountQAR { get; set; }
    public decimal TotalQAR { get; set; }
    public decimal PaidAmountQAR { get; set; }
    public decimal OutstandingAmountQAR { get; set; }
    public string? Notes { get; set; }
    public Guid? ReportFormatId { get; set; }

    // Navigation Properties
    public Client? Client { get; set; }
    public Quotation? Quotation { get; set; }
    public ICollection<InvoiceLineItem> LineItems { get; set; } = [];
    public ICollection<InvoicePayment> Payments { get; set; } = [];
    public ICollection<InvoiceTripLink> TripLinks { get; set; } = [];
    public InvoiceReportFormat? ReportFormat { get; set; }
    public ICollection<InvoiceReminderLog> ReminderLogs { get; set; } = [];
}
