using Application.Abstractions.Messaging;
using Application.Trips.Common;

namespace Application.Trips.GetTrips;

public sealed record GetTripsQuery : IQuery<List<TripResponse>>;
