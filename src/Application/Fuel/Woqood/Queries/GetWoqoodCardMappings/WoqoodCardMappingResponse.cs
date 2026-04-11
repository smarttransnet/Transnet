namespace Application.Fuel.Woqood.Queries.GetWoqoodCardMappings;

public sealed record WoqoodCardMappingResponse(
    Guid Id,
    string CardNumber,
    Guid? VehicleId,
    string? VehiclePlate,
    Guid? DriverId,
    string? DriverName,
    string CardHolderName,
    bool IsActive,
    string? Notes
);
