using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Billing;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.InvoiceReportFormats.Commands.UpdateReportFormat;

internal sealed class UpdateReportFormatCommandHandler(
    IApplicationDbContext dbContext)
    : ICommandHandler<UpdateReportFormatCommand>
{
    public async Task<Result> Handle(UpdateReportFormatCommand request, CancellationToken cancellationToken)
    {
        InvoiceReportFormat? format = await dbContext.InvoiceReportFormats
            .Include(f => f.ReportColumns)
            .FirstOrDefaultAsync(f => f.Id == request.ReportFormatId, cancellationToken);

        if (format is null)
        {
            return Result.Failure(Error.NotFound("ReportFormat.NotFound", "The specified report format was not found."));
        }

        bool nameExists = await dbContext.InvoiceReportFormats
            .AnyAsync(f => f.Name == request.Name && f.Id != request.ReportFormatId, cancellationToken);

        if (nameExists)
        {
            return Result.Failure(Error.Conflict("ReportFormat.NameInUse", "Another report format with the specified name already exists."));
        }

        if (request.IsDefault && !format.IsDefault)
        {
            List<InvoiceReportFormat> existingDefaults = await dbContext.InvoiceReportFormats
                .Where(f => f.IsDefault && f.Id != request.ReportFormatId)
                .ToListAsync(cancellationToken);

            foreach (InvoiceReportFormat existing in existingDefaults)
            {
                existing.IsDefault = false;
            }
        }

        format.Name = request.Name;
        format.Description = request.Description;
        format.ShowShippingAddress = request.ShowShippingAddress;
        format.ShowTaxBreakdown = request.ShowTaxBreakdown;
        format.ShowTripDetails = request.ShowTripDetails;
        format.ColumnConfiguration = request.ColumnConfiguration;
        format.HeaderLogoUrl = request.HeaderLogoUrl;
        format.FooterText = request.FooterText;
        format.IsDefault = request.IsDefault;
        format.IsActive = request.IsActive;

        // Sync columns
        dbContext.ReportFormatColumns.RemoveRange(format.ReportColumns);
        
        foreach (Application.InvoiceReportFormats.Commands.CreateReportFormat.ReportFormatColumnRequest colRequest in request.Columns)
        {
            format.ReportColumns.Add(new ReportFormatColumn
            {
                Id = Guid.NewGuid(),
                InvoiceReportFormatId = format.Id,
                ColumnKey = colRequest.ColumnKey,
                DisplayLabel = colRequest.DisplayLabel,
                WidthPercent = colRequest.WidthPercent,
                IsVisible = colRequest.IsVisible,
                SortOrder = colRequest.SortOrder,
                FormatPattern = colRequest.FormatPattern
            });
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
