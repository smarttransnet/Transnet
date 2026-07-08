using Application.Abstractions.Messaging;
using Domain.Assets.Enums;

namespace Application.Assets.UpdateVehicle;

public sealed record UpdateVehicleCommand(
    Guid VehicleId,
    string PlateNumber,
    string ChassisNumber,
    string Make,
    string Model,
    int Year,
    Guid VehicleCategoryId,
    VehicleType VehicleType,
    VehicleStatus Status,
    decimal OdometerReading,
    bool IsActive) : ICommand;
