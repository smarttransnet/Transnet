using Application.Abstractions.Messaging;
using Domain.Assets.Enums;

namespace Application.Assets.RegisterVehicle;

public sealed record RegisterVehicleCommand(
    string RegistrationNumber,
    string PlateNumber,
    string Make,
    string Model,
    int Year,
    Guid VehicleCategoryId,
    VehicleType VehicleType,
    VehicleStatus Status,
    decimal OdometerReading) : ICommand<Guid>;
