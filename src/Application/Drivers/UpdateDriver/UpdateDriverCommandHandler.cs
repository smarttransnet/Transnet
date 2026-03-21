using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Drivers;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Drivers.UpdateDriver;

internal sealed class UpdateDriverCommandHandler(
    IApplicationDbContext dbContext,
    IDateTimeProvider dateTimeProvider)
    : ICommandHandler<UpdateDriverCommand>
{
    public async Task<Result> Handle(UpdateDriverCommand request, CancellationToken cancellationToken)
    {
        Driver? driver = await dbContext.Drivers
            .FirstOrDefaultAsync(d => d.Id == request.DriverId, cancellationToken);

        if (driver is null)
        {
            return Result.Failure(Error.NotFound("Drivers.NotFound", $"The driver with the Id = '{request.DriverId}' was not found"));
        }

        driver.EmployeeNumber = request.EmployeeNumber;
        driver.FirstName = request.FirstName;
        driver.LastName = request.LastName;
        driver.PhoneNumber = request.PhoneNumber;
        driver.LicenceNumber = request.LicenceNumber;
        driver.LicenceExpiryDate = request.LicenceExpiryDate;
        driver.NationalityCode = request.NationalityCode;
        driver.Status = request.Status;
        driver.IsActive = request.IsActive;
        driver.Email = request.Email;
        driver.SponsorName = request.SponsorName;
        driver.UpdatedAt = dateTimeProvider.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
