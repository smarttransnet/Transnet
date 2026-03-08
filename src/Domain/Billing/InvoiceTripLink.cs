using Domain.Trips;
using SharedKernel;

namespace Domain.Billing;

public sealed class InvoiceTripLink : Entity
{
    public Guid Id { get; set; }
    public Guid InvoiceId { get; set; }
    public Guid TripId { get; set; }
    public DateTime LinkedAt { get; set; }
    public Guid LinkedByUserId { get; set; }
    public bool TripCompletionVerified { get; set; }

    // Navigation Properties
    public Invoice? Invoice { get; set; }
    public Trip? Trip { get; set; }
}
