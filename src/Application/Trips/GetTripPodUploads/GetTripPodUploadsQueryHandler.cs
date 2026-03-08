using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Trips.Common;
using Domain.Trips;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Trips.GetTripPodUploads;

internal sealed class GetTripPodUploadsQueryHandler : IQueryHandler<GetTripPodUploadsQuery, List<TripPodUploadResponse>>
{
    private readonly IApplicationDbContext _context;

    public GetTripPodUploadsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<TripPodUploadResponse>>> Handle(GetTripPodUploadsQuery request, CancellationToken cancellationToken)
    {
        List<TripPodUploadResponse> uploads = await _context.TripPodUploads
            .Where(p => p.TripId == request.TripId)
            .OrderByDescending(p => p.UploadedAt)
            .Select(p => new TripPodUploadResponse(
                p.Id,
                p.TripId,
                p.TripStopId,
                p.DocumentType,
                new Uri(p.FileUrl),
                p.FileName,
                p.Latitude,
                p.Longitude,
                p.UploadedAt,
                p.UploadedByDriverId))
            .ToListAsync(cancellationToken);

        return Result.Success(uploads);
    }
}
