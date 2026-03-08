using Domain.Trips.Enums;
using SharedKernel;

namespace Domain.Trips;

public sealed class ImportBatch : Entity
{
    public Guid Id { get; set; }
    public string FileName { get; set; }
    public Guid UploadedByUserId { get; set; }
    public DateTime UploadedAt { get; set; }
    public int TotalRows { get; set; }
    public int SuccessCount { get; set; }
    public int FailureCount { get; set; }
    public ImportStatus Status { get; set; }
    public string? ErrorSummary { get; set; }

    public ICollection<Trip> ImportedTrips { get; set; } = new List<Trip>();
}
