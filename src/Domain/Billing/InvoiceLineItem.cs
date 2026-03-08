using Domain.Billing.Enums;
using Domain.Trips;
using SharedKernel;

namespace Domain.Billing;

public sealed class InvoiceLineItem : Entity
{
    public Guid Id { get; set; }
    public Guid InvoiceId { get; set; }
    public Guid? TripId { get; set; }
    public string Description { get; set; } = string.Empty;
    public ServiceType ServiceType { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPriceQAR { get; set; }
    public decimal DiscountPercent { get; set; }
    public decimal TaxPercent { get; set; }
    public decimal LineTotalQAR { get; set; }
    public int SortOrder { get; set; }

    // Navigation Properties
    public Invoice? Invoice { get; set; }
    public Trip? Trip { get; set; }
}
