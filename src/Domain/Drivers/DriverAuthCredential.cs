using Domain.Drivers.Enums;
using SharedKernel;

namespace Domain.Drivers;

public sealed class DriverAuthCredential : Entity
{
    public Guid Id { get; set; }
    public Guid DriverId { get; set; }
    public string UsernameHash { get; set; }
    public string PasswordHash { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiresAt { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public string? DeviceToken { get; set; }
    public MobilePlatform Platform { get; set; }
    public bool IsLocked { get; set; }
    public int FailedAttempts { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation Property
    public Driver Driver { get; set; }
}
