using Application.Abstractions.Messaging;
using Domain.Drivers;
using Domain.Drivers.Enums;
namespace Application.Drivers.CreateDriver; public sealed record CreateDriverCommand(string EmployeeNumber, string FirstName, string LastName, string PhoneNumber, string LicenceNumber, DateOnly LicenceExpiryDate, string NationalityCode, string? Email = null, string? SponsorName = null, DriverStatus Status = DriverStatus.Active) : ICommand<Guid>;
