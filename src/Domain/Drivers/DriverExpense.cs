using Domain.Drivers.Enums;
using SharedKernel;

namespace Domain.Drivers;

public sealed class DriverExpense : Entity
{
    public Guid Id { get; set; }
    public Guid DriverId { get; set; }
    public Guid? TripId { get; set; }
    public ExpenseType ExpenseType { get; set; }
    public decimal AmountQAR { get; set; }
    public DateOnly ExpenseDate { get; set; }
    public string? Description { get; set; }
    public string? ReceiptUrl { get; set; }
    
    // Fuel specific fields
    public decimal? FuelLitres { get; set; }
    public string? FuelStation { get; set; }
    public decimal? OdometerReading { get; set; }
    
    public ExpenseStatus Status { get; set; }
    public Guid? ReviewedByUserId { get; set; }
    public DateTime? ReviewedAt { get; set; }
    public DateTime SubmittedAt { get; set; }

    // Navigation Property
    public Driver Driver { get; set; }
}
