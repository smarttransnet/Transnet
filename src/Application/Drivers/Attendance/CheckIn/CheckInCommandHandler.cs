using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Drivers;
using Domain.Drivers.Enums;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Drivers.Attendance.CheckIn;

internal sealed class CheckInCommandHandler(
    IApplicationDbContext dbContext,
    IDateTimeProvider dateTimeProvider) : ICommandHandler<CheckInCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CheckInCommand request, CancellationToken cancellationToken)
    {
        bool driverExists = await dbContext.Drivers.AnyAsync(d => d.Id == request.DriverId, cancellationToken);
        if (!driverExists)
        {
            return Result.Failure<Guid>(DriverErrors.NotFound(request.DriverId));
        }

        var today = DateOnly.FromDateTime(dateTimeProvider.UtcNow.Date);

        // Check if already checked in today
        bool alreadyCheckedIn = await dbContext.DriverAttendanceLogs
            .AnyAsync(l => l.DriverId == request.DriverId && l.AttendanceDate == today && l.CheckOutAt == null, cancellationToken);

        if (alreadyCheckedIn)
        {
            return Result.Failure<Guid>(Error.Conflict("Attendance.Conflict", "Driver is already checked in and has not checked out."));
        }

        var attendanceLog = new DriverAttendanceLog
        {
            Id = Guid.NewGuid(),
            DriverId = request.DriverId,
            AttendanceDate = today,
            CheckInAt = dateTimeProvider.UtcNow,
            CheckInLatitude = request.Latitude,
            CheckInLongitude = request.Longitude,
            Notes = request.Notes,
            Source = request.Source ?? AttendanceSource.DriverApp
        };

        dbContext.DriverAttendanceLogs.Add(attendanceLog);
        await dbContext.SaveChangesAsync(cancellationToken);

        return attendanceLog.Id;
    }
}

