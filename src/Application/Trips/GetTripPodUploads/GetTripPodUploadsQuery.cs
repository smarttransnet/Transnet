using Application.Abstractions.Messaging;
using Application.Trips.Common;

namespace Application.Trips.GetTripPodUploads;

public sealed record GetTripPodUploadsQuery(Guid TripId) : IQuery<List<TripPodUploadResponse>>;
