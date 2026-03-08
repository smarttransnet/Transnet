namespace Application.Trips.Common;

public sealed record CustomFieldDefinitionResponse(
    Guid Id,
    string FieldName,
    string FieldLabel,
    string DataType,
    string FieldType,
    string AppliesTo,
    bool IsRequired,
    string? DefaultValue,
    string? ValidationRegex,
    bool IsActive,
    int SortOrder);
