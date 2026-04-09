namespace Domain.Billing.Enums;

public enum InvoiceStatus
{
    Draft,
    Issued,
    PartiallyPaid,
    Paid,
    Overdue,
    Cancelled,
    Disputed,
    Generated
}
