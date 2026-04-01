using Application.Abstractions.Messaging;
using Domain.Drivers;
using Domain.Drivers.Enums;
namespace Application.Drivers.UpdateDriver; public sealed record UpdateDriverCommand(Guid DriverId, string EmployeeNumber, string FirstName, string LastName, string? PhoneNumber, string LicenceNumber, DateOnly LicenceExpiryDate, string NationalityCode, DriverStatus Status, bool IsActive, string? Email = null, string? SponsorName = null) : ICommand;
