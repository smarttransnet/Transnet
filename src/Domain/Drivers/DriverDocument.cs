using Domain.Drivers.Enums;
using SharedKernel;

namespace Domain.Drivers;

public sealed class DriverDocument : Entity
{
    public Guid Id { get; set; }
    public Guid DriverId { get; set; }
    public Guid? TripId { get; set; }
    public DriverDocumentType DocumentType { get; set; }
    public string Title { get; set; }
    public string FileUrl { get; set; }
    public DateTime UploadedAt { get; set; }
    public bool SubmittedFromApp { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }

    // Navigation Property
    public Driver Driver { get; set; }
}
