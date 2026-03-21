using Application.Abstractions.Messaging;
using Application.Quotations.Commands.UpdateQuotation;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.ClientBilling.Quotations;

internal sealed class Update : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("quotations/{id:guid}", async (
            Guid id,
            UpdateQuotationCommand request,
            ICommandHandler<UpdateQuotationCommand> handler,
            CancellationToken cancellationToken) =>
        {
            if (id != request.Id)
            {
                return Results.BadRequest("Mismatched ID");
            }

            Result result = await handler.Handle(request, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.ClientBilling);
    }
}
