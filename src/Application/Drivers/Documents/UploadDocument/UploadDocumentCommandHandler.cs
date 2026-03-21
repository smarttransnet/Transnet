using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Drivers;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Drivers.Documents.UploadDocument;

internal sealed class UploadDocumentCommandHandler(
    IApplicationDbContext dbContext,
    IDateTimeProvider dateTimeProvider)
    : ICommandHandler<UploadDocumentCommand, Guid>
{
    public async Task<Result<Guid>> Handle(UploadDocumentCommand request, CancellationToken cancellationToken)
    {
        if (!await dbContext.Drivers.AnyAsync(d => d.Id == request.DriverId, cancellationToken))
        {
            return Result.Failure<Guid>(Error.NotFound("Drivers.NotFound", $"The driver with the Id = '{request.DriverId}' was not found"));
        }

        var document = new DriverDocument
        {
            Id = Guid.NewGuid(),
            DriverId = request.DriverId,
            TripId = request.TripId,
            DocumentType = request.DocumentType,
            Title = request.Title,
            FileUrl = request.FileUrl.ToString(),
            SubmittedFromApp = request.SubmittedFromApp,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            UploadedAt = dateTimeProvider.UtcNow
        };

        dbContext.DriverDocuments.Add(document);

        await dbContext.SaveChangesAsync(cancellationToken);

        return document.Id;
    }
}
