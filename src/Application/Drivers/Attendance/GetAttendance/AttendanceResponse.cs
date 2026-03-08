using Application.Abstractions.Messaging;
using Domain.Drivers;
using Domain.Drivers.Enums;
namespace Application.Drivers.Attendance.GetAttendance; public sealed record AttendanceResponse(Guid Id, Guid DriverId, DateOnly AttendanceDate, DateTime? CheckInAt, DateTime? CheckOutAt, decimal? TotalHoursWorked, string? Notes, AttendanceSource Source);
