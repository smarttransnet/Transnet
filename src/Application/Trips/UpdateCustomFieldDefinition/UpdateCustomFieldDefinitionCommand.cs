using Application.Abstractions.Messaging;

namespace Application.Trips.UpdateCustomFieldDefinition;

public sealed record UpdateCustomFieldDefinitionCommand(
    Guid Id,
    string FieldName,
    string FieldType,
    bool IsRequired,
    string? DefaultValue,
    string? ValidationRegex) : ICommand;
