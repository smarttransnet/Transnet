using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Drivers;
using SharedKernel;

namespace Application.Drivers.CreateDriver;

internal sealed class CreateDriverCommandHandler(
    IApplicationDbContext dbContext,
    IDateTimeProvider dateTimeProvider)
    : ICommandHandler<CreateDriverCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateDriverCommand request, CancellationToken cancellationToken)
    {
        var driver = new Driver
        {
            Id = Guid.NewGuid(),
            EmployeeNumber = request.EmployeeNumber,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email,
            LicenceNumber = request.LicenceNumber,
            LicenceExpiryDate = request.LicenceExpiryDate,
            NationalityCode = request.NationalityCode,
            SponsorName = request.SponsorName,
            Status = request.Status,
            IsActive = true,
            CreatedAt = dateTimeProvider.UtcNow,
            UpdatedAt = dateTimeProvider.UtcNow
        };

        dbContext.Drivers.Add(driver);

        await dbContext.SaveChangesAsync(cancellationToken);

        return driver.Id;
    }
}
