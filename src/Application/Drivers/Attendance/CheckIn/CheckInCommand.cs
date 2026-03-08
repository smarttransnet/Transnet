using Application.Abstractions.Messaging;
using Domain.Drivers.Enums;

namespace Application.Drivers.Attendance.CheckIn;

public sealed record CheckInCommand(
    Guid DriverId,
    double? Latitude = null,
    double? Longitude = null,
    string? Notes = null,
    AttendanceSource? Source = null) : ICommand<Guid>;
