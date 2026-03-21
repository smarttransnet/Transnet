using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Drivers;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Drivers.GetDriverById;

internal sealed class GetDriverByIdQueryHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetDriverByIdQuery, DriverResponse>
{
    public async Task<Result<DriverResponse>> Handle(GetDriverByIdQuery request, CancellationToken cancellationToken)
    {
        DriverResponse? driver = await dbContext.Drivers
            .AsNoTracking()
            .Where(d => d.Id == request.DriverId)
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
            return Result.Failure<DriverResponse>(Error.NotFound("Drivers.NotFound", $"The driver with the Id = '{request.DriverId}' was not found"));
        }

        return driver;
    }
}
