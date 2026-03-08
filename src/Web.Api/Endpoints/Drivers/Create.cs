using Application.Abstractions.Messaging;
using Application.Drivers.CreateDriver;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Drivers;

internal sealed class Create : IEndpoint
{
    public sealed record Request(
        string EmployeeNumber,
        string FirstName,
        string LastName,
        string PhoneNumber,
        string LicenceNumber,
        DateOnly LicenceExpiryDate,
        string NationalityCode,
        string? Email = null,
        string? SponsorName = null);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("drivers", async (
            Request request,
            ICommandHandler<CreateDriverCommand, Guid> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new CreateDriverCommand(
                request.EmployeeNumber,
                request.FirstName,
                request.LastName,
                request.PhoneNumber,
                request.LicenceNumber,
                request.LicenceExpiryDate,
                request.NationalityCode,
                request.Email,
                request.SponsorName);

            Result<Guid> result = await handler.Handle(command, cancellationToken);
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Drivers);
    }
}
