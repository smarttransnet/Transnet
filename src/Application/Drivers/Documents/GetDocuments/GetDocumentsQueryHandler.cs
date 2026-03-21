using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Drivers;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Drivers.Documents.GetDocuments;

internal sealed class GetDocumentsQueryHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetDocumentsQuery, PagedList<DocumentResponse>>
{
    public async Task<Result<PagedList<DocumentResponse>>> Handle(GetDocumentsQuery request, CancellationToken cancellationToken)
    {
        IQueryable<DriverDocument> documentsQuery = dbContext.DriverDocuments;

        if (request.DriverId.HasValue)
        {
            documentsQuery = documentsQuery.Where(d => d.DriverId == request.DriverId.Value);
        }

        if (request.TripId.HasValue)
        {
            documentsQuery = documentsQuery.Where(d => d.TripId == request.TripId.Value);
        }

        if (request.DocumentType.HasValue)
        {
            documentsQuery = documentsQuery.Where(d => d.DocumentType == request.DocumentType.Value);
        }

        int totalCount = await documentsQuery.CountAsync(cancellationToken);

        List<DocumentResponse> documents = await documentsQuery
            .OrderByDescending(d => d.UploadedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(d => new DocumentResponse(
                d.Id,
                d.DriverId,
                d.TripId,
                d.DocumentType,
                d.Title,
                new Uri(d.FileUrl),
                d.UploadedAt,
                d.SubmittedFromApp,
                d.Latitude,
                d.Longitude))
            .ToListAsync(cancellationToken);

        return new PagedList<DocumentResponse>(documents, totalCount, request.Page, request.PageSize);
    }
}
