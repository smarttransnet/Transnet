using Application.Abstractions.Messaging;
using Application.Trips.GetCustomFieldDefinitions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Trips;

public sealed class GetCustomFieldDefinitions : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("custom-field-definitions", async (
            IQueryHandler<GetCustomFieldDefinitionsQuery, List<Application.Trips.Common.CustomFieldDefinitionResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetCustomFieldDefinitionsQuery();

            Result<List<Application.Trips.Common.CustomFieldDefinitionResponse>> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Trips);
    }
}
