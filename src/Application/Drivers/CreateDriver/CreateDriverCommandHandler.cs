using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Drivers;
using SharedKernel;

namespace Application.Drivers.CreateDriver;

internal sealed class CreateDriverCommandHandler(IApplicationDbContext context)
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
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        context.Drivers.Add(driver);

        await context.SaveChangesAsync(cancellationToken);

        return driver.Id;
    }
}
