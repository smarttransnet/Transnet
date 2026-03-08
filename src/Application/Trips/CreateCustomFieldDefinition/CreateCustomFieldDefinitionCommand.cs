using Application.Abstractions.Messaging;

namespace Application.Trips.CreateCustomFieldDefinition;

public sealed record CreateCustomFieldDefinitionCommand(
    string FieldName,
    string FieldType,
    bool IsRequired,
    string? DefaultValue,
    string? ValidationRegex) : ICommand<Guid>;
