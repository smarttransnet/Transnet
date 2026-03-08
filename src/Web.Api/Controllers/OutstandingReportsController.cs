using Application.OutstandingReports.Commands.GenerateOutstandingReport;
using Application.OutstandingReports.Queries.GetOutstandingReports;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/outstanding-reports")]
public class OutstandingReportsController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetOutstandingReports(
        [FromQuery] Guid? clientId,
        [FromQuery] int? periodMonth,
        [FromQuery] int? periodYear,
        CancellationToken cancellationToken)
    {
        var query = new GetOutstandingReportsQuery(clientId, periodMonth, periodYear);
        SharedKernel.Result<IReadOnlyList<OutstandingReportResponse>> result = await sender.Send(query, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [HttpPost]
    public async Task<IActionResult> GenerateOutstandingReport([FromBody] GenerateOutstandingReportCommand command, CancellationToken cancellationToken)
    {
        SharedKernel.Result<Guid> result = await sender.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(new { ReportId = result.Value }) : BadRequest(result.Error);
    }
}
