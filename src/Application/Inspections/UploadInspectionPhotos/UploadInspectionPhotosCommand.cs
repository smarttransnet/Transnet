using Application.Abstractions.Messaging;
using Domain.Inspections.Enums;

namespace Application.Inspections.UploadInspectionPhotos;

public sealed record PhotoUploadItem(
    Stream Content,
    string FileName,
    string? Caption,
    PhotoType PhotoType);

public sealed record UploadInspectionPhotosCommand(
    Guid InspectionId,
    List<PhotoUploadItem> Photos) : ICommand;
