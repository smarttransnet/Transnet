using Domain.Billing.Enums;
using SharedKernel;

namespace Domain.Billing;

public sealed class InvoicePayment : Entity
{
    public Guid Id { get; set; }
    public Guid InvoiceId { get; set; }
    public decimal AmountQAR { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public string? PaymentReference { get; set; }
    public DateOnly PaymentDate { get; set; }
    public Guid RecordedByUserId { get; set; }
    public string? Notes { get; set; }

    // Navigation Properties
    public Invoice? Invoice { get; set; }
}
