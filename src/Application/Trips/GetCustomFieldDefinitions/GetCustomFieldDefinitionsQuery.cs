using Application.Abstractions.Messaging;
using Application.Trips.Common;

namespace Application.Trips.GetCustomFieldDefinitions;

public sealed record GetCustomFieldDefinitionsQuery() : IQuery<List<CustomFieldDefinitionResponse>>;
