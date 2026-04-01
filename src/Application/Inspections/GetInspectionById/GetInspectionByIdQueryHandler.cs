using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Inspections;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Inspections.GetInspectionById;

internal sealed class GetInspectionByIdQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetInspectionByIdQuery, InspectionDetailedResponse>
{
    public async Task<Result<InspectionDetailedResponse>> Handle(
        GetInspectionByIdQuery query,
        CancellationToken cancellationToken)
    {
        var inspection = await context.VehicleInspections
            .Include(i => i.Vehicle)
            .Include(i => i.InspectionChecklist)
            .Include(i => i.InspectionResults)
                .ThenInclude(r => r.ChecklistItem)
            .Include(i => i.Photos)
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.Id == query.Id, cancellationToken);

        if (inspection is null)
        {
            return Result.Failure<InspectionDetailedResponse>(Error.NotFound(
                "Inspection.NotFound",
                $"The inspection with the ID '{query.Id}' was not found."));
        }

        // We might need to fetch driver name from another context if not in the same DB 
        // For now, we assume it's just the ID or we can fetch it if Driver entity exists in this context.
        // Let's check Domain to see if Driver entity exists.
        
        var driver = await context.Drivers
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.Id == inspection.DriverId, cancellationToken);
            
        var response = new InspectionDetailedResponse(
            inspection.Id,
            inspection.VehicleId,
            inspection.Vehicle.RegistrationNumber,
            inspection.InspectionChecklistId,
            inspection.InspectionChecklist.Name,
            inspection.InspectionType,
            inspection.DriverId,
            driver != null ? $"{driver.FirstName} {driver.LastName}" : "Unknown Driver",
            inspection.InspectedAt,
            inspection.DriverSignature,
            inspection.DriverSignedAt,
            inspection.Notes,
            inspection.OdometerReading,
            inspection.Status,
            inspection.InspectionResults.Select(r => new InspectionResultResponse(
                r.Id,
                r.ChecklistItemId,
                r.ChecklistItem.ItemName,
                r.IsPassed,
                r.Remarks)).ToList(),
            inspection.Photos.Select(p => new InspectionPhotoResponse(
                p.Id,
                p.PhotoPath,
                p.Caption,
                p.PhotoType,
                p.UploadedAt)).ToList());

        return response;
    }
}
