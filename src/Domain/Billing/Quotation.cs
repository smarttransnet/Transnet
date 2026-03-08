using Domain.Billing.Enums;
using Domain.Clients;
using SharedKernel;

namespace Domain.Billing;

public sealed class Quotation : Entity
{
    public Guid Id { get; set; }
    public string QuotationNumber { get; set; } = string.Empty;
    public Guid ClientId { get; set; }
    public Guid IssuedByUserId { get; set; }
    public DateTime IssuedAt { get; set; }
    public DateOnly ValidUntil { get; set; }
    public QuotationStatus Status { get; set; }
    public decimal SubTotalQAR { get; set; }
    public decimal TaxAmountQAR { get; set; }
    public decimal TotalQAR { get; set; }
    public string? Notes { get; set; }
    public string? TermsAndConditions { get; set; }
    public Guid? ConvertedToInvoiceId { get; set; }

    // Navigation Properties
    public Client? Client { get; set; }
    public ICollection<QuotationLineItem> LineItems { get; set; } = [];
    public Invoice? ConvertedInvoice { get; set; }
}
