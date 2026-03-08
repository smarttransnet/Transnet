using Application.Abstractions.Messaging;
using Application.Trips.Common;

namespace Application.Trips.GetStopsByTripId;

public sealed record GetStopsByTripIdQuery(Guid TripId) : IQuery<List<TripStopResponse>>;
