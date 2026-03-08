using Application.Abstractions.Messaging;
using Application.Trips.Common;

namespace Application.Trips.GetHaltsByTripId;

public sealed record GetHaltsByTripIdQuery(Guid TripId) : IQuery<List<TripHaltResponse>>;
