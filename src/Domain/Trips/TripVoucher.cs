using SharedKernel;

namespace Domain.Trips;

public sealed class TripVoucher : Entity
{
    public Guid Id { get; set; }
    public Guid TripId { get; set; }
    public string VoucherNumber { get; set; }
    public DateTime VoucherDate { get; set; }
    public string? Notes { get; set; }
    public Guid CreatedByUserId { get; set; }
    public DateTime CreatedAt { get; set; }

    public Trip Trip { get; set; }
    public ICollection<TripCustomField> CustomFields { get; set; } = new List<TripCustomField>();
}
