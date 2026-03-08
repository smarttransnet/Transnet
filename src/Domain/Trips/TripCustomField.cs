using SharedKernel;

namespace Domain.Trips;

public sealed class TripCustomField : Entity
{
    public Guid Id { get; set; }
    public Guid TripId { get; set; }
    public Guid? TripVoucherId { get; set; }
    public Guid FieldDefinitionId { get; set; }
    public string Value { get; set; }

    public Trip Trip { get; set; }
    public CustomFieldDefinition FieldDefinition { get; set; }
}
