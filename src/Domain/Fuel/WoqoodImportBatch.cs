using Domain.Fuel.Enums;
using SharedKernel;

namespace Domain.Fuel;

public sealed class WoqoodImportBatch : Entity
{
    public Guid Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public Guid UploadedByUserId { get; set; }
    public DateTime UploadedAt { get; set; }
    public int PeriodMonth { get; set; }
    public int PeriodYear { get; set; }
    public int TotalRows { get; set; }
    public int SuccessCount { get; set; }
    public int FailureCount { get; set; }
    public decimal TotalLitres { get; set; }
    public decimal TotalAmountQAR { get; set; }
    public ImportStatus Status { get; set; }
    public string? ErrorSummary { get; set; }

    // Navigation Properties
    public ICollection<WoqoodFuelTransaction> Transactions { get; set; } = [];
}
