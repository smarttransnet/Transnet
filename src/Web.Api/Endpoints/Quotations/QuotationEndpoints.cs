using Application.Abstractions.Messaging;
using Application.Quotations.Commands.CreateQuotation;
using Application.Quotations.Commands.ConvertQuotationToInvoice;
using Application.Quotations.Commands.DeleteQuotation;
using Application.Quotations.Commands.UpdateQuotation;
using Application.Quotations.Queries.GetQuotationById;
using Application.Quotations.Queries.GetQuotations;
using Microsoft.AspNetCore.Http;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Quotations;

internal sealed class QuotationEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("quotations").WithTags(Tags.Quotations);

        group.MapPost("", async (
            CreateQuotationCommand command,
            ICommandHandler<CreateQuotationCommand, Guid> handler,
            CancellationToken cancellationToken) =>
        {
            Result<Guid> result = await handler.Handle(command, cancellationToken);
            return result.IsSuccess ? Results.Created($"/quotations/{result.Value}", result.Value) : CustomResults.Problem(result);
        });

        group.MapGet("", async (
            IQueryHandler<GetQuotationsQuery, IReadOnlyList<QuotationResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetQuotationsQuery();
            Result<IReadOnlyList<QuotationResponse>> result = await handler.Handle(query, cancellationToken);
            return result.IsSuccess ? Results.Ok(result.Value) : CustomResults.Problem(result);
        });

        group.MapGet("{id}", async (
            Guid id,
            IQueryHandler<GetQuotationByIdQuery, QuotationDetailResponse> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetQuotationByIdQuery(id);
            Result<QuotationDetailResponse> result = await handler.Handle(query, cancellationToken);
            return result.IsSuccess ? Results.Ok(result.Value) : CustomResults.Problem(result);
        });

        group.MapPut("{id}", async (
            Guid id,
            UpdateQuotationCommand command,
            ICommandHandler<UpdateQuotationCommand> handler,
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
            ICommandHandler<DeleteQuotationCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new DeleteQuotationCommand(id);
            Result result = await handler.Handle(command, cancellationToken);
            return result.IsSuccess ? Results.NoContent() : CustomResults.Problem(result);
        });

        group.MapPost("{id}/convert", async (
            Guid id,
            ConvertQuotationToInvoiceCommand command,
            ICommandHandler<ConvertQuotationToInvoiceCommand, Guid> handler,
            CancellationToken cancellationToken) =>
        {
            if (id != command.QuotationId)
            {
                return Results.BadRequest("ID mismatch");
            }
            Result<Guid> result = await handler.Handle(command, cancellationToken);
            return result.IsSuccess ? Results.Ok(result.Value) : CustomResults.Problem(result);
        });
    }
}
