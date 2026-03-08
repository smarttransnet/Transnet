using Application.Abstractions.Messaging;

namespace Application.InvoiceReportFormats.Queries.GetReportFormats;

public sealed record GetReportFormatsQuery(
    bool? IsActive = null
) : IQuery<IReadOnlyList<ReportFormatResponse>>;
