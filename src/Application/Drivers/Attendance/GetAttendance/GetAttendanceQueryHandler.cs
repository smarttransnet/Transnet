using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Extensions;
using Domain.Drivers;
using Domain.Drivers.Enums;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Drivers.Attendance.GetAttendance;

internal sealed class GetAttendanceQueryHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetAttendanceQuery, PagedList<AttendanceResponse>>
{
    public async Task<Result<PagedList<AttendanceResponse>>> Handle(GetAttendanceQuery request, CancellationToken cancellationToken)
    {
        IQueryable<DriverAttendanceLog> query = dbContext.DriverAttendanceLogs
            .AsNoTracking()
            .Where(l => l.DriverId == request.DriverId);

        if (request.StartDate.HasValue)
        {
            query = query.Where(l => l.AttendanceDate >= request.StartDate.Value);
        }
        else if (!request.EndDate.HasValue)
        {
            var thirtyDaysAgo = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-30));
            query = query.Where(l => l.AttendanceDate >= thirtyDaysAgo);
        }

        if (request.EndDate.HasValue)
        {
            query = query.Where(l => l.AttendanceDate <= request.EndDate.Value);
        }

        IQueryable<AttendanceResponse> responsesQuery = query
            .OrderByDescending(l => l.AttendanceDate)
            .Select(l => new AttendanceResponse(
                l.Id,
                l.DriverId,
                l.AttendanceDate,
                l.CheckInAt,
                l.CheckOutAt,
                l.TotalHoursWorked,
                l.Notes,
                l.Source));

        return await responsesQuery.ToPagedListAsync(
            request.Page,
            request.PageSize,
            cancellationToken);
    }
}
