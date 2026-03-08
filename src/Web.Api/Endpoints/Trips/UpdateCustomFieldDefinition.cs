using Application.Abstractions.Messaging;
using Application.Trips.UpdateCustomFieldDefinition;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Trips;

public sealed class UpdateCustomFieldDefinition : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("custom-field-definitions/{id:guid}", async (
            Guid id, 
            UpdateCustomFieldDefinitionRequest request, 
            ICommandHandler<UpdateCustomFieldDefinitionCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new UpdateCustomFieldDefinitionCommand(
                id,
                request.FieldName,
                request.FieldType,
                request.IsRequired,
                request.DefaultValue,
                request.ValidationRegex);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.Trips);
    }
}

public sealed record UpdateCustomFieldDefinitionRequest(
    string FieldName,
    string FieldType,
    bool IsRequired,
    string? DefaultValue,
    string? ValidationRegex);
