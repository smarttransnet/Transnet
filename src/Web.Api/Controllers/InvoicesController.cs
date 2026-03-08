using Application.Invoices.Commands.CreateInvoice;
using Application.Invoices.Commands.LinkTripToInvoice;
using Application.Invoices.Commands.RecordPayment;
using Application.Invoices.Queries.GetInvoiceById;
using Application.Invoices.Queries.GetInvoices;
using Domain.Billing.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/invoices")]
public class InvoicesController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetInvoices(
        [FromQuery] Guid? clientId,
        [FromQuery] InvoiceStatus? status,
        [FromQuery] string? searchTerm,
        CancellationToken cancellationToken)
    {
        var query = new GetInvoicesQuery(clientId, status, searchTerm);
        SharedKernel.Result<IReadOnlyList<InvoiceResponse>> result = await sender.Send(query, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetInvoiceById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetInvoiceByIdQuery(id);
        SharedKernel.Result<InvoiceDetailResponse> result = await sender.Send(query, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
    }

    [HttpPost]
    public async Task<IActionResult> CreateInvoice([FromBody] CreateInvoiceCommand command, CancellationToken cancellationToken)
    {
        SharedKernel.Result<Guid> result = await sender.Send(command, cancellationToken);
        return result.IsSuccess ? CreatedAtAction(nameof(GetInvoiceById), new { id = result.Value }, result.Value) : BadRequest(result.Error);
    }

    [HttpPost("{id:guid}/payments")]
    public async Task<IActionResult> RecordPayment(Guid id, [FromBody] RecordPaymentCommand command, CancellationToken cancellationToken)
    {
        if (id != command.InvoiceId)
        {
            return BadRequest();
        }

        SharedKernel.Result<Guid> result = await sender.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(new { PaymentId = result.Value }) : BadRequest(result.Error);
    }

    [HttpPost("{id:guid}/trips")]
    public async Task<IActionResult> LinkTrip(Guid id, [FromBody] LinkTripToInvoiceCommand command, CancellationToken cancellationToken)
    {
        if (id != command.InvoiceId)
        {
            return BadRequest();
        }

        SharedKernel.Result result = await sender.Send(command, cancellationToken);
        return result.IsSuccess ? NoContent() : BadRequest(result.Error);
    }
}
