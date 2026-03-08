namespace Application.Trips.Common;

public sealed record TripVoucherResponse(
    Guid Id,
    Guid TripId,
    string VoucherNumber,
    DateTime VoucherDate,
    string? Notes,
    Guid CreatedByUserId,
    DateTime CreatedAt,
    List<TripCustomFieldResponse> CustomFields);
