using Domain.Assets;
using Domain.Drivers;
using SharedKernel;

namespace Domain.Fuel;

public sealed class WoqoodCardMapping : Entity
{
    public Guid Id { get; set; }
    public string WoqoodCardNumber { get; set; } = string.Empty;
    public Guid? VehicleId { get; set; }
    public Guid? DriverId { get; set; }
    public string CardHolderName { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public DateTime MappedAt { get; set; }
    public Guid MappedByUserId { get; set; }
    public string? Notes { get; set; }

    // Navigation Properties
    public Vehicle? Vehicle { get; set; }
    public Driver? Driver { get; set; }
}
