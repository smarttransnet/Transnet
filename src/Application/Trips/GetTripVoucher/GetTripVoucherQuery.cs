using Application.Abstractions.Messaging;
using Application.Trips.Common;

namespace Application.Trips.GetTripVoucher;

public sealed record GetTripVoucherQuery(Guid TripId) : IQuery<TripVoucherResponse>;
