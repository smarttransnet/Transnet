using Application.Abstractions.Messaging;
using Application.Trips.Common;

namespace Application.Invoices.Queries.GetAvailableTrips;

public sealed record GetAvailableTripsQuery(Guid ClientId) : IQuery<IReadOnlyList<TripResponse>>;
