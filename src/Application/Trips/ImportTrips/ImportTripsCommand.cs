using Application.Abstractions.Messaging;

namespace Application.Trips.ImportTrips;

public sealed record ImportTripsCommand(
    string FileName,
    byte[] FileContent,
    Guid UploadedByUserId) : ICommand<Guid>;
