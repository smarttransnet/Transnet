namespace Domain.Trips.Enums;

public enum TripStatus
{
    Scheduled = 1,
    InProgress = 2,
    OnHalt = 3,
    Completed = 4,
    PendingDriverConfirmation = 5,
    PendingOfficeApproval = 6,
    Cancelled = 7,
    Invoiced = 8
}
