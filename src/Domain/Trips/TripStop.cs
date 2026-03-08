using Domain.Trips.Enums;
using SharedKernel;

namespace Domain.Trips;

public sealed class TripStop : Entity
{
    public Guid Id { get; set; }
    public Guid TripId { get; set; }
    public int StopOrder { get; set; }
    public StopType StopType { get; set; }
    public string LocationName { get; set; }
    public string? Address { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public string? PocName { get; set; }
    public string? PocPhone { get; set; }
    public string? PocEmail { get; set; }
    public DateTime? ScheduledArrivalAt { get; set; }
    public DateTime? ActualArrivalAt { get; set; }
    public DateTime? ActualDepartureAt { get; set; }
    public string? Notes { get; set; }

    public Trip Trip { get; set; }
    public ICollection<TripPodUpload> PodUploads { get; set; } = new List<TripPodUpload>();
}
