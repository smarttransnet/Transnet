using Domain.Drivers.Enums;
using SharedKernel;

namespace Domain.Drivers;

public sealed class DriverNotification : Entity
{
    public Guid Id { get; set; }
    public Guid DriverId { get; set; }
    public NotificationType NotificationType { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public Guid? RelatedEntityId { get; set; }
    public string? RelatedEntityType { get; set; }
    public bool IsRead { get; set; }
    public DateTime? ReadAt { get; set; }
    public DateTime SentAt { get; set; }
    public NotificationChannel Channel { get; set; }

    // Navigation Property
    public Driver Driver { get; set; }
}
