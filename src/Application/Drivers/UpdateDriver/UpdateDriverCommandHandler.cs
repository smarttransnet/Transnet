using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Drivers.UpdateDriver;

internal sealed class UpdateDriverCommandHandler(IApplicationDbContext context)
    : ICommandHandler<UpdateDriverCommand>
{
    public async Task<Result> Handle(UpdateDriverCommand request, CancellationToken cancellationToken)
    {
        var driver = await context.Drivers
            .FirstOrDefaultAsync(d => d.Id == request.DriverId, cancellationToken);

        if (driver is null)
        {
            return Result.Failure(Error.NotFound(
                "Driver.NotFound",
                $"The driver with the ID '{request.DriverId}' was not found."));
        }

        driver.EmployeeNumber = request.EmployeeNumber;
        driver.FirstName = request.FirstName;
        driver.LastName = request.LastName;
        driver.PhoneNumber = request.PhoneNumber;
        driver.Email = request.Email;
        driver.LicenceNumber = request.LicenceNumber;
        driver.LicenceExpiryDate = request.LicenceExpiryDate;
        driver.NationalityCode = request.NationalityCode;
        driver.SponsorName = request.SponsorName;
        driver.Status = request.Status;
        driver.IsActive = request.IsActive;
        driver.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
