using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Inspections;
using SharedKernel;

namespace Application.Inspections.UploadInspectionPhotos;

internal sealed class UploadInspectionPhotosCommandHandler(IApplicationDbContext context)
    : ICommandHandler<UploadInspectionPhotosCommand>
{
    public async Task<Result> Handle(
        UploadInspectionPhotosCommand command,
        CancellationToken cancellationToken)
    {
        var inspection = await context.VehicleInspections
            .FindAsync([command.InspectionId], cancellationToken);

        if (inspection is null)
        {
            return Result.Failure(Error.NotFound(
                "Inspection.NotFound",
                $"The inspection with the ID '{command.InspectionId}' was not found."));
        }

        foreach (var photoItem in command.Photos)
        {
            // In a real app, we would upload to blob storage and get a URL
            var photoPath = $"/uploads/inspections/{command.InspectionId}/{photoItem.FileName}";
            
            var photo = new InspectionPhoto
            {
                Id = Guid.NewGuid(),
                VehicleInspectionId = command.InspectionId,
                PhotoPath = photoPath,
                Caption = photoItem.Caption,
                PhotoType = photoItem.PhotoType,
                UploadedAt = DateTime.UtcNow,
                UploadedByDriverId = inspection.DriverId // or current user
            };

            inspection.Photos.Add(photo);
        }

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
