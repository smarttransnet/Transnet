namespace Application.Fuel.Woqood.Queries.GetWoqoodCardMappings;

public sealed record WoqoodCardMappingResponse(
    Guid Id,
    string WoqoodCardNumber,
    Guid? VehicleId,
    Guid? DriverId,
    string CardHolderName,
    bool IsActive,
    string? Notes
);
