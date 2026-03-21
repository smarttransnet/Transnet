namespace Application.Fuel.Woqood;

public sealed record WoqoodCardMappingResponse(
    Guid Id,
    string WoqoodCardNumber,
    Guid? VehicleId,
    Guid? DriverId,
    string CardHolderName,
    bool IsActive,
    string? Notes);
