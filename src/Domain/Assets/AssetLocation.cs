using Domain.Assets.Enums;
using Domain.WorkOrders.Enums;
using SharedKernel;

namespace Domain.Assets;

public sealed class AssetLocation : Entity
{
    public Guid Id { get; set; }
    public Guid AssetId { get; set; }
    public AssetType AssetType { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string? LocationName { get; set; }
    public bool IsAssigned { get; set; }
    public DateTime RecordedAt { get; set; }
    public LocationSource Source { get; set; }
}
