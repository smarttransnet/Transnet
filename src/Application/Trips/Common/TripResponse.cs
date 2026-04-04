using Domain.Trips.Enums;

namespace Application.Trips.Common;

public sealed record TripResponse(
    Guid Id,
    string TripNumber,
    Guid DriverId,
    Guid VehicleId,
    Guid? TrailerId,
    TripStatus Status,
    DateTime ScheduledStartAt,
    DateTime? ActualStartAt,
    DateTime? ActualEndAt,
    decimal? TotalDistanceKm,
    bool IsImported,
    Guid? ImportBatchId,
    string Origin,
    string Destination,
    DateTime? DriverConfirmedAt,
    DateTime? OfficeApprovedAt,
    Guid? OfficeApprovedByUserId,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    List<TripStopResponse>? Stops = null,
    List<TripHaltResponse>? Halts = null,
    TripVoucherResponse? Voucher = null,
    List<TripPodUploadResponse>? PodUploads = null);
