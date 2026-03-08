using Domain.Billing.Enums;
using SharedKernel;

namespace Domain.Billing;

public sealed class QuotationLineItem : Entity
{
    public Guid Id { get; set; }
    public Guid QuotationId { get; set; }
    public string Description { get; set; } = string.Empty;
    public ServiceType ServiceType { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPriceQAR { get; set; }
    public decimal DiscountPercent { get; set; }
    public decimal TaxPercent { get; set; }
    public decimal LineTotalQAR { get; set; }
    public int SortOrder { get; set; }

    // Navigation Properties
    public Quotation? Quotation { get; set; }
}
