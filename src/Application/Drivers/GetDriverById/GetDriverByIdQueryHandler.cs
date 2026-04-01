using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Drivers.GetDriverById;

internal sealed class GetDriverByIdQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetDriverByIdQuery, DriverResponse>
{
    public async Task<Result<DriverResponse>> Handle(GetDriverByIdQuery query, CancellationToken cancellationToken)
    {
        var driver = await context.Drivers
            .AsNoTracking()
            .Where(d => d.Id == query.DriverId)
            .Select(d => new DriverResponse(
                d.Id,
                d.EmployeeNumber,
                d.FirstName,
                d.LastName,
                d.PhoneNumber,
                d.Email,
                d.LicenceNumber,
                d.LicenceExpiryDate,
                d.NationalityCode,
                d.SponsorName,
                (int)d.Status,
                d.IsActive,
                d.CreatedAt,
                d.UpdatedAt))
            .FirstOrDefaultAsync(cancellationToken);

        if (driver is null)
        {
            return Result.Failure<DriverResponse>(Error.NotFound(
                "Driver.NotFound",
                $"The driver with the ID '{query.DriverId}' was not found."));
        }

        return driver;
    }
}
