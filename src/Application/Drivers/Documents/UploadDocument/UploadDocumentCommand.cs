using Application.Abstractions.Messaging;
using Domain.Drivers.Enums;

namespace Application.Drivers.Documents.UploadDocument;

public sealed record UploadDocumentCommand(
    Guid DriverId,
    Guid? TripId,
    DriverDocumentType DocumentType,
    string Title,
    Uri FileUrl,
    bool SubmittedFromApp,
    double? Latitude,
    double? Longitude) : ICommand<Guid>;
