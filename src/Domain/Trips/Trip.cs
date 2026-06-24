using Domain.Trips.Enums;
using Domain.Clients;
using SharedKernel;

namespace Domain.Trips;

public sealed class Trip : Entity
{
    public Guid Id { get; set; }
    public string TripNumber { get; set; }
    public Guid DriverId { get; set; }
    public Guid VehicleId { get; set; }
    public Guid? TrailerId { get; set; }
    public Guid? ClientId { get; set; }
    public string Origin { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public TripStatus Status { get; set; }
    public DateTime ScheduledStartAt { get; set; }
    public DateTime? ActualStartAt { get; set; }
    public DateTime? ActualEndAt { get; set; }
    public decimal? TotalDistanceKm { get; set; }
    public bool IsImported { get; set; }
    public Guid? ImportBatchId { get; set; }
    public DateTime? DriverConfirmedAt { get; set; }
    public DateTime? OfficeApprovedAt { get; set; }
    public Guid? OfficeApprovedByUserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string? SuptNo { get; set; }
    public string? SuptDocPath { get; set; }
    public Guid? TripCategoryMaterialId { get; set; }

    // Navigation Properties
    public Client? Client { get; set; }
    public TripCategoryMaterial? TripCategoryMaterial { get; set; }
    public ICollection<TripStop> Stops { get; set; } = new List<TripStop>();
    public ICollection<TripHalt> Halts { get; set; } = new List<TripHalt>();
    public TripVoucher? Voucher { get; set; }
    public ICollection<TripPodUpload> PodUploads { get; set; } = new List<TripPodUpload>();
    public ICollection<TripStatusHistory> StatusHistory { get; set; } = new List<TripStatusHistory>();
    public ICollection<TripCustomField> CustomFields { get; set; } = new List<TripCustomField>();
}
