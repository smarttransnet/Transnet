using Application.Abstractions.Messaging;

namespace Application.Trips.CreateTripVoucher;

public sealed record CreateTripVoucherCommand(
    Guid TripId,
    string VoucherNumber,
    DateTime VoucherDate,
    string? Notes,
    Guid CreatedByUserId) : ICommand<Guid>;
