using Domain.Assets.Enums;

namespace Application.Assets.GetVehicles;

public sealed record VehicleResponse(
    Guid Id,
    string RegistrationNumber,
    string PlateNumber,
    string Make,
    string Model,
    int Year,
    Guid VehicleCategoryId,
    VehicleType VehicleType,
    VehicleStatus Status,
    Guid? CurrentDriverId,
    Guid? CurrentLocationId,
    decimal OdometerReading,
    bool IsActive);
