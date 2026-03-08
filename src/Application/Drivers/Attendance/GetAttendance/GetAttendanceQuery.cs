using Application.Abstractions.Messaging;
using Domain.Drivers;
using SharedKernel;
namespace Application.Drivers.Attendance.GetAttendance; public sealed record GetAttendanceQuery(Guid DriverId, DateOnly? StartDate, DateOnly? EndDate, int Page = 1, int PageSize = 10) : IQuery<PagedList<AttendanceResponse>>;
