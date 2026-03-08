using Application.Abstractions.Messaging;
using Domain.Drivers.Enums;

namespace Application.Drivers.Attendance.CheckOut;

public sealed record CheckOutCommand(
    Guid DriverId,
    double? Latitude = null,
    double? Longitude = null,
    string? Notes = null,
    AttendanceSource? Source = null) : ICommand;
