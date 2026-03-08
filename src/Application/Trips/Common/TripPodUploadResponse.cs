using Domain.Trips.Enums;

namespace Application.Trips.Common;

public sealed record TripPodUploadResponse(
    Guid Id,
    Guid TripId,
    Guid? TripStopId,
    PodDocumentType DocumentType,
    Uri FileUrl,
    string FileName,
    double? Latitude,
    double? Longitude,
    DateTime UploadedAt,
    Guid UploadedByDriverId);
