using Application.Abstractions.Messaging;

namespace Application.Trips.UpdateTripVoucher;

public sealed record UpdateTripVoucherCommand(
    Guid TripId,
    string VoucherNumber,
    DateTime VoucherDate,
    string? Notes) : ICommand;
