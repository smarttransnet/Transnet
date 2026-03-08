using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Trips;
using Domain.Trips.Enums;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Trips.UploadTripPod;

internal sealed class UploadTripPodCommandHandler : ICommandHandler<UploadTripPodCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public UploadTripPodCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Guid>> Handle(UploadTripPodCommand request, CancellationToken cancellationToken)
    {
        bool tripExists = await _context.Trips.AnyAsync(t => t.Id == request.TripId, cancellationToken);
        if (!tripExists)
        {
            return Result.Failure<Guid>(TripErrors.NotFound(request.TripId));
        }

        TripPodUpload upload = new()
        {
            Id = Guid.NewGuid(),
            TripId = request.TripId,
            TripStopId = request.TripStopId,
            DocumentType = request.DocumentType,
            FileUrl = request.FileUrl.ToString(),
            FileName = request.FileName,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            UploadedAt = DateTime.UtcNow,
            UploadedByDriverId = request.UploadedByDriverId
        };

        _context.TripPodUploads.Add(upload);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(upload.Id);
    }
}
