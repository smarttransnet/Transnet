using Application.Abstractions.Messaging;
using Application.Trips.Common;

namespace Application.Trips.GetTripById;

public sealed record GetTripByIdQuery(Guid Id) : IQuery<TripResponse>;
