using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Drivers;
using Domain.Drivers.Enums;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Drivers.Attendance.CheckOut;

internal sealed class CheckOutCommandHandler(
    IApplicationDbContext dbContext,
    IDateTimeProvider dateTimeProvider) : ICommandHandler<CheckOutCommand>
{
    public async Task<Result> Handle(CheckOutCommand request, CancellationToken cancellationToken)
    {
        var today = DateOnly.FromDateTime(dateTimeProvider.UtcNow.Date);

        DriverAttendanceLog? existingLog = await dbContext.DriverAttendanceLogs
            .FirstOrDefaultAsync(l => l.DriverId == request.DriverId && l.AttendanceDate == today && l.CheckOutAt == null, cancellationToken);

        if (existingLog is null)
        {
            return Result.Failure(Error.NotFound("Attendance.NotFound", "No active check-in found for today."));
        }

        existingLog.CheckOutAt = dateTimeProvider.UtcNow;
        existingLog.CheckOutLatitude = request.Latitude;
        existingLog.CheckOutLongitude = request.Longitude;

        if (!string.IsNullOrWhiteSpace(request.Notes))
        {
            existingLog.Notes += "\n" + request.Notes;
        }

        if (existingLog.CheckInAt.HasValue && existingLog.CheckOutAt.HasValue)
        {
            TimeSpan duration = existingLog.CheckOutAt.Value - existingLog.CheckInAt.Value;
            existingLog.TotalHoursWorked = (decimal)Math.Round(duration.TotalHours, 2);
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
