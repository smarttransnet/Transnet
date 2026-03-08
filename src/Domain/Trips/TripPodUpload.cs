using Domain.Trips.Enums;
using SharedKernel;

namespace Domain.Trips;

public sealed class TripPodUpload : Entity
{
    public Guid Id { get; set; }
    public Guid TripId { get; set; }
    public Guid? TripStopId { get; set; }
    public PodDocumentType DocumentType { get; set; }
    public string FileUrl { get; set; }
    public string FileName { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public DateTime UploadedAt { get; set; }
    public Guid UploadedByDriverId { get; set; }

    public Trip Trip { get; set; }
    public TripStop? TripStop { get; set; }
}
