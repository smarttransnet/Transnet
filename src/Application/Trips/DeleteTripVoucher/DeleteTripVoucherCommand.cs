using Application.Abstractions.Messaging;

namespace Application.Trips.DeleteTripVoucher;

public sealed record DeleteTripVoucherCommand(Guid TripId) : ICommand;
