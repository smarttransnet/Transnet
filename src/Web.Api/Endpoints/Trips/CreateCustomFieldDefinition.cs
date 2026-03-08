using Application.Abstractions.Messaging;
using Application.Trips.CreateCustomFieldDefinition;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Trips;

public sealed class CreateCustomFieldDefinition : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("custom-field-definitions", async (
            CreateCustomFieldDefinitionRequest request, 
            ICommandHandler<CreateCustomFieldDefinitionCommand, Guid> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new CreateCustomFieldDefinitionCommand(
                request.FieldName,
                request.FieldType,
                request.IsRequired,
                request.DefaultValue,
                request.ValidationRegex);

            Result<Guid> result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Trips);
    }
}

public sealed record CreateCustomFieldDefinitionRequest(
    string FieldName,
    string FieldType,
    bool IsRequired,
    string? DefaultValue,
    string? ValidationRegex);
