using SharedKernel;

namespace Domain.Trips;

public sealed class CustomFieldDefinition : Entity
{
    public Guid Id { get; set; }
    public string FieldName { get; set; }
    public string FieldLabel { get; set; }
    public string DataType { get; set; }
    public string FieldType { get; set; } // Same as DataType but used in handlers
    public string AppliesTo { get; set; }
    public bool IsRequired { get; set; }
    public string? DefaultValue { get; set; }
    public string? ValidationRegex { get; set; }
    public bool IsActive { get; set; }
    public int SortOrder { get; set; }
}
