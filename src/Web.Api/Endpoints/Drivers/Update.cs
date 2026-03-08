using Application.Abstractions.Messaging;
using Application.Drivers.UpdateDriver;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Drivers;

internal sealed class Update : IEndpoint
{
    public sealed record Request(
        string EmployeeNumber,
        string FirstName,
        string LastName,
        string PhoneNumber,
        string LicenceNumber,
        DateOnly LicenceExpiryDate,
        string NationalityCode,
        Domain.Drivers.Enums.DriverStatus Status,
        bool IsActive,
        string? Email = null,
        string? SponsorName = null);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("drivers/{id:guid}", async (
            Guid id,
            Request request,
            ICommandHandler<UpdateDriverCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new UpdateDriverCommand(
                id,
                request.EmployeeNumber,
                request.FirstName,
                request.LastName,
                request.PhoneNumber,
                request.LicenceNumber,
                request.LicenceExpiryDate,
                request.NationalityCode,
                request.Status,
                request.IsActive,
                request.Email,
                request.SponsorName);

            Result result = await handler.Handle(command, cancellationToken);
            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.Drivers);
    }
}
