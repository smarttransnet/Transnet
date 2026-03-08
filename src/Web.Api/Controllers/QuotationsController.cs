using Application.Quotations.Commands.ConvertQuotationToInvoice;
using Application.Quotations.Commands.CreateQuotation;
using Application.Quotations.Queries.GetQuotationById;
using Application.Quotations.Queries.GetQuotations;
using Domain.Billing.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/quotations")]
public class QuotationsController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetQuotations(
        [FromQuery] Guid? clientId,
        [FromQuery] QuotationStatus? status,
        [FromQuery] string? searchTerm,
        CancellationToken cancellationToken)
    {
        var query = new GetQuotationsQuery(clientId, status, searchTerm);
        SharedKernel.Result<IReadOnlyList<QuotationResponse>> result = await sender.Send(query, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetQuotationById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetQuotationByIdQuery(id);
        SharedKernel.Result<QuotationDetailResponse> result = await sender.Send(query, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
    }

    [HttpPost]
    public async Task<IActionResult> CreateQuotation([FromBody] CreateQuotationCommand command, CancellationToken cancellationToken)
    {
        SharedKernel.Result<Guid> result = await sender.Send(command, cancellationToken);
        return result.IsSuccess ? CreatedAtAction(nameof(GetQuotationById), new { id = result.Value }, result.Value) : BadRequest(result.Error);
    }

    [HttpPost("{id:guid}/convert")]
    public async Task<IActionResult> ConvertToInvoice(Guid id, [FromBody] ConvertQuotationToInvoiceCommand command, CancellationToken cancellationToken)
    {
        if (id != command.QuotationId)
        {
            return BadRequest();
        }

        SharedKernel.Result<Guid> result = await sender.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(new { InvoiceId = result.Value }) : BadRequest(result.Error);
    }
}
