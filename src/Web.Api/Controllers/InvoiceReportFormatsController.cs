using Application.InvoiceReportFormats.Commands.CreateReportFormat;
using Application.InvoiceReportFormats.Commands.UpdateReportFormat;
using Application.InvoiceReportFormats.Queries.GetReportFormats;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/invoice-report-formats")]
public class InvoiceReportFormatsController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetReportFormats(
        [FromQuery] bool? isActive,
        CancellationToken cancellationToken)
    {
        var query = new GetReportFormatsQuery(isActive);
        SharedKernel.Result<IReadOnlyList<ReportFormatResponse>> result = await sender.Send(query, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [HttpPost]
    public async Task<IActionResult> CreateReportFormat([FromBody] CreateReportFormatCommand command, CancellationToken cancellationToken)
    {
        SharedKernel.Result<Guid> result = await sender.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(new { Id = result.Value }) : BadRequest(result.Error);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateReportFormat(Guid id, [FromBody] UpdateReportFormatCommand command, CancellationToken cancellationToken)
    {
        if (id != command.ReportFormatId)
        {
            return BadRequest();
        }

        SharedKernel.Result result = await sender.Send(command, cancellationToken);
        return result.IsSuccess ? NoContent() : BadRequest(result.Error);
    }
}
