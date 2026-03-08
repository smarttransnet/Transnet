using Application.Abstractions.Messaging;
using Application.Drivers.Expenses.UpdateExpense;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.DriverExpenses;

internal sealed class Update : IEndpoint
{
    public sealed record Request(decimal Amount, DateOnly ExpenseDate, string Description, Uri? ReceiptUrl);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("drivers/expenses/{id:guid}", async (
            Guid id,
            Request request,
            ICommandHandler<UpdateExpenseCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new UpdateExpenseCommand(id, request.Amount, request.ExpenseDate, request.Description, request.ReceiptUrl);
            Result result = await handler.Handle(command, cancellationToken);
            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.DriverExpenses);
    }
}
