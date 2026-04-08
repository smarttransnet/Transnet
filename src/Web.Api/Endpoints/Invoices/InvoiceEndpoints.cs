using Application.Abstractions.Messaging;
using Application.Invoices.Commands.CreateInvoice;
using Application.Invoices.Commands.DeleteInvoice;
using Application.Invoices.Commands.UpdateInvoice;
using Application.Invoices.Commands.IssueInvoice;
using Application.Invoices.Commands.CancelInvoice;
using Application.Invoices.Queries.GetInvoiceById;
using Application.Invoices.Queries.GetInvoices;
using MediatR;
using Microsoft.AspNetCore.Http;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Invoices;

internal sealed class InvoiceEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("invoices").WithTags(Tags.Invoices);

        group.MapPost("", async (
            CreateInvoiceCommand command,
            ICommandHandler<CreateInvoiceCommand, Guid> handler,
            CancellationToken cancellationToken) =>
        {
            Result<Guid> result = await handler.Handle(command, cancellationToken);
            return result.IsSuccess ? Results.Created($"/invoices/{result.Value}", result.Value) : CustomResults.Problem(result);
        });

        group.MapGet("", async (
            IQueryHandler<GetInvoicesQuery, IReadOnlyList<InvoiceResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetInvoicesQuery();
            Result<IReadOnlyList<InvoiceResponse>> result = await handler.Handle(query, cancellationToken);
            return result.IsSuccess ? Results.Ok(result.Value) : CustomResults.Problem(result);
        });

        group.MapGet("{id}", async (
            Guid id,
            IQueryHandler<GetInvoiceByIdQuery, InvoiceDetailResponse> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetInvoiceByIdQuery(id);
            Result<InvoiceDetailResponse> result = await handler.Handle(query, cancellationToken);
            return result.IsSuccess ? Results.Ok(result.Value) : CustomResults.Problem(result);
        });

        group.MapPut("{id}", async (
            Guid id,
            UpdateInvoiceCommand command,
            ICommandHandler<UpdateInvoiceCommand> handler,
            CancellationToken cancellationToken) =>
        {
            if (id != command.Id)
            {
                return Results.BadRequest("ID mismatch");
            }
            Result result = await handler.Handle(command, cancellationToken);
            return result.IsSuccess ? Results.NoContent() : CustomResults.Problem(result);
        });

        group.MapDelete("{id}", async (
            Guid id,
            ICommandHandler<DeleteInvoiceCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new DeleteInvoiceCommand(id);
            Result result = await handler.Handle(command, cancellationToken);
            return result.IsSuccess ? Results.NoContent() : CustomResults.Problem(result);
        });

        group.MapPut("{id}/issue", async (
            Guid id,
            ICommandHandler<IssueInvoiceCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new IssueInvoiceCommand(id);
            Result result = await handler.Handle(command, cancellationToken);
            return result.IsSuccess ? Results.NoContent() : CustomResults.Problem(result);
        });

        group.MapPut("{id}/cancel", async (
            Guid id,
            ICommandHandler<CancelInvoiceCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new CancelInvoiceCommand(id);
            Result result = await handler.Handle(command, cancellationToken);
            return result.IsSuccess ? Results.NoContent() : CustomResults.Problem(result);
        });
    }
}
