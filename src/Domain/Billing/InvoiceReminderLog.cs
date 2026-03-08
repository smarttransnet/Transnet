using Domain.Billing.Enums;
using SharedKernel;

namespace Domain.Billing;

public sealed class InvoiceReminderLog : Entity
{
    public Guid Id { get; set; }
    public Guid InvoiceId { get; set; }
    public DateTime SentAt { get; set; }
    public string SentToEmail { get; set; } = string.Empty;
    public ReminderType ReminderType { get; set; }
    public ReminderDeliveryStatus DeliveryStatus { get; set; }
    public string? DeliveryError { get; set; }
    public Guid? TriggeredByUserId { get; set; }
    public bool IsAutomated { get; set; }

    // Navigation Properties
    public Invoice? Invoice { get; set; }
}
