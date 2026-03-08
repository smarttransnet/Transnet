using Application.Abstractions.Messaging;
using Domain.Drivers;
using Domain.Drivers.Enums;
using SharedKernel;
namespace Application.Drivers.Documents.GetDocuments; public sealed record GetDocumentsQuery(Guid? DriverId = null, Guid? TripId = null, DriverDocumentType? DocumentType = null, int Page = 1, int PageSize = 10) : IQuery<PagedList<DocumentResponse>>;
