using Application.Abstractions.Messaging;

namespace Application.Inspections.SignInspection;

public sealed record SignInspectionCommand(
    Guid InspectionId,
    string SignerName,
    string SignatureData, // Base64 or typed name
    DateTime SignedAt) : ICommand;
