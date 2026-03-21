using Application.Abstractions.Messaging;
using Application.Quotations.Commands.DeleteQuotation;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.ClientBilling.Quotations;

internal sealed class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("quotations/{id}", async (
            Guid id,
            ICommandHandler<DeleteQuotationCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new DeleteQuotationCommand(id);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.ClientBilling);
    }
}
