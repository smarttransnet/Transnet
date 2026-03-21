using Application.Abstractions.Messaging;
using Application.Drivers.Documents.GetDocuments;
using Domain.Drivers.Enums;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.DriverDocuments;

internal sealed class GetAll : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("drivers/{driverId:guid}/documents", async (
            Guid driverId,
            [FromQuery] Guid? tripId,
            [FromQuery] DriverDocumentType? type,
            [FromQuery] int? page,
            [FromQuery] int? pageSize,
            IQueryHandler<GetDocumentsQuery, PagedList<DocumentResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetDocumentsQuery(driverId, tripId, type, page ?? 1, pageSize ?? 10);
            Result<PagedList<DocumentResponse>> result = await handler.Handle(query, cancellationToken);
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.DriverDocuments);
    }
}
