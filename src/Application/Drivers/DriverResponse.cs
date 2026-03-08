using Application.Abstractions.Messaging;
using Domain.Drivers;
namespace Application.Drivers; public sealed record DriverResponse(Guid Id, string EmployeeNumber, string FirstName, string LastName, string PhoneNumber, string? Email, string LicenceNumber, DateOnly LicenceExpiryDate, string NationalityCode, string? SponsorName, int Status, bool IsActive, DateTime CreatedAt, DateTime UpdatedAt);
