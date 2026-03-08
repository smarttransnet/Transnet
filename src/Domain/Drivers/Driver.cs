using Domain.Drivers.Enums;
using SharedKernel;

namespace Domain.Drivers;

public sealed class Driver : Entity
{
    public Guid Id { get; set; }
    public string EmployeeNumber { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string LicenceNumber { get; set; }
    public DateOnly LicenceExpiryDate { get; set; }
    public string NationalityCode { get; set; }
    public string? SponsorName { get; set; }
    public DriverStatus Status { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation Properties
    public DriverAuthCredential? AuthCredential { get; set; }
    public ICollection<DriverAttendanceLog> AttendanceLogs { get; set; } = [];
    public ICollection<DriverExpense> Expenses { get; set; } = [];
    public ICollection<DriverLocationUpdate> LocationUpdates { get; set; } = [];
    public ICollection<DriverNotification> Notifications { get; set; } = [];
    public ICollection<DriverDocument> Documents { get; set; } = [];
}
