using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Billing;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.InvoiceReportFormats.Commands.CreateReportFormat;

internal sealed class CreateReportFormatCommandHandler(
    IApplicationDbContext dbContext)
    : ICommandHandler<CreateReportFormatCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateReportFormatCommand request, CancellationToken cancellationToken)
    {
        bool nameExists = await dbContext.InvoiceReportFormats
            .AnyAsync(f => f.Name == request.Name, cancellationToken);

        if (nameExists)
        {
            return Result.Failure<Guid>(Error.Conflict("ReportFormat.NameInUse", "A report format with the specified name already exists."));
        }

        if (request.IsDefault)
        {
            List<InvoiceReportFormat> existingDefaults = await dbContext.InvoiceReportFormats
                .Where(f => f.IsDefault)
                .ToListAsync(cancellationToken);

            foreach (InvoiceReportFormat existing in existingDefaults)
            {
                existing.IsDefault = false;
            }
        }

        var format = new InvoiceReportFormat
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            ShowShippingAddress = request.ShowShippingAddress,
            ShowTaxBreakdown = request.ShowTaxBreakdown,
            ShowTripDetails = request.ShowTripDetails,
            ColumnConfiguration = request.ColumnConfiguration,
            HeaderLogoUrl = request.HeaderLogoUrl,
            FooterText = request.FooterText,
            IsDefault = request.IsDefault,
            IsActive = true
        };

        foreach (ReportFormatColumnRequest colRequest in request.Columns)
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

        dbContext.InvoiceReportFormats.Add(format);
        await dbContext.SaveChangesAsync(cancellationToken);

        return format.Id;
    }
}
