using Application.Abstractions.Messaging;
using Domain.Trips.Enums;

namespace Application.Trips.UploadTripPod;

public sealed record UploadTripPodCommand(
    Guid TripId,
    Guid? TripStopId,
    PodDocumentType DocumentType,
    Uri FileUrl,
    string FileName,
    double? Latitude,
    double? Longitude,
    Guid UploadedByDriverId) : ICommand<Guid>;
