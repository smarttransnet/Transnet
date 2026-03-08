namespace Application.Trips.Common;

public sealed record TripCustomFieldResponse(
    Guid Id,
    Guid TripId,
    Guid? TripVoucherId,
    Guid FieldDefinitionId,
    string Value);
