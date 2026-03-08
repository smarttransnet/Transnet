using Application.Abstractions.Messaging;
using Application.Drivers.Expenses.GetExpenses;
using Domain.Drivers.Enums;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.DriverExpenses;

internal sealed class GetAll : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("drivers/{driverId:guid}/expenses", async (
            Guid driverId,
            [FromQuery] Guid? tripId,
            [FromQuery] ExpenseStatus? status,
            [FromQuery] DateOnly? startDate,
            [FromQuery] DateOnly? endDate,
            [FromQuery] int page,
            [FromQuery] int pageSize,
            IQueryHandler<GetExpensesQuery, PagedList<ExpenseResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetExpensesQuery(driverId, tripId, status, startDate, endDate, page, pageSize);
            Result<PagedList<ExpenseResponse>> result = await handler.Handle(query, cancellationToken);
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.DriverExpenses);
    }
}
