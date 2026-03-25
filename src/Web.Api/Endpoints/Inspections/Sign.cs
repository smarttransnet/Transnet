using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Application.Abstractions.Messaging;
using Application.Inspections.SignInspection;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Inspections;

internal sealed class Sign : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("inspections/{id}/sign", async (
            Guid id,
            SignInspectionCommand request,
            [FromServices] ICommandHandler<SignInspectionCommand> handler,
            CancellationToken cancellationToken) =>
        {
            if (id != request.InspectionId)
            {
                return Results.BadRequest("ID mismatch");
            }

            Result result = await handler.Handle(request, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.Inspections);
    }
}
