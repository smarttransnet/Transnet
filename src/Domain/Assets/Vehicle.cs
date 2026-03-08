using Domain.Assets.Enums;
using SharedKernel;

namespace Domain.Assets;

public sealed class Vehicle : Entity
{
    public Guid Id { get; set; }
    public string RegistrationNumber { get; set; }
    public string PlateNumber { get; set; }
    public string Make { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    public Guid VehicleCategoryId { get; set; }
    public VehicleType VehicleType { get; set; }
    public VehicleStatus Status { get; set; }
    public Guid? CurrentDriverId { get; set; }
    public Guid? CurrentLocationId { get; set; }
    public decimal OdometerReading { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsActive { get; set; }

    public VehicleCategory Category { get; set; }
}
