using Application.Abstractions.Messaging;
using Domain.Drivers;
using Domain.Drivers.Enums;
namespace Application.Drivers.Documents.GetDocuments; public sealed record DocumentResponse(Guid Id, Guid DriverId, Guid? TripId, DriverDocumentType DocumentType, string Title, Uri FileUrl, DateTime UploadedAt, bool SubmittedFromApp, double? Latitude, double? Longitude);
