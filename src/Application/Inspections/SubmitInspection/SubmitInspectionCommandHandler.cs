using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Inspections;
using Domain.Inspections.Enums;
using SharedKernel;

namespace Application.Inspections.SubmitInspection;

internal sealed class SubmitInspectionCommandHandler(
    IApplicationDbContext dbContext,
    IDateTimeProvider dateTimeProvider)
    : ICommandHandler<SubmitInspectionCommand, Guid>
{
    public async Task<Result<Guid>> Handle(SubmitInspectionCommand request, CancellationToken cancellationToken)
    {
        InspectionStatus status = request.InspectionResults.Any(r => r.Status == "Faulty") ? InspectionStatus.ActionRequired : InspectionStatus.Submitted;
        DateTime now = dateTimeProvider.UtcNow;

        var inspection = new VehicleInspection
        {
            Id = Guid.NewGuid(),
            VehicleId = request.VehicleId,
            InspectionChecklistId = request.InspectionChecklistId,
            InspectionType = request.InspectionType,
            DriverId = request.DriverId,
            TripId = request.TripId,
            InspectedAt = now,
            DriverSignature = request.DriverSignature,
            DriverSignedAt = string.IsNullOrWhiteSpace(request.DriverSignature) ? null : now,
            Notes = request.Notes,
            OdometerReading = request.OdometerReading,
            Status = status,
            InspectionResults = request.InspectionResults.Select(r => new InspectionResult
            {
                Id = Guid.NewGuid(),
                ChecklistItemId = r.ChecklistItemId,
                Status = r.Status,
                Remarks = r.Remarks,
                RecordedAt = now
            }).ToList()
        };

        dbContext.VehicleInspections.Add(inspection);

        await dbContext.SaveChangesAsync(cancellationToken);

        return inspection.Id;
    }
}
