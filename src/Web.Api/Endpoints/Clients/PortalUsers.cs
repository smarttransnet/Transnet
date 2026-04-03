using Application.Abstractions.Messaging;
using Application.Clients.Commands.CreatePortalUser;
using Application.Clients.Commands.UpdatePortalUser;
using Application.Clients.Queries.GetPortalUsers;
using Domain.Clients.Enums;
using MediatR;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Clients;

internal sealed class PortalUsers : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("clients/{id}/portal-users", async (
            Guid id,
            bool? isActive,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var query = new GetPortalUsersQuery(id, isActive);
            Result<IReadOnlyList<ClientPortalUserResponse>> result = await sender.Send(query, cancellationToken);
            return result.IsSuccess ? Results.Ok(result.Value) : CustomResults.Problem(result);
        })
        .WithTags(Tags.Clients);

        app.MapPost("clients/{id}/portal-users", async (
            Guid id,
            CreatePortalUserRequest request,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var command = new CreatePortalUserCommand(
                id,
                request.FullName,
                request.Email,
                request.PlainTextPassword,
                Enum.Parse<ClientPortalRole>(request.Role));

            Result<Guid> result = await sender.Send(command, cancellationToken);
            return result.IsSuccess ? Results.Created($"/clients/{id}/portal-users/{result.Value}", result.Value) : CustomResults.Problem(result);
        })
        .WithTags(Tags.Clients);

        app.MapPut("clients/{id}/portal-users/{userId}", async (
            Guid id,
            Guid userId,
            UpdatePortalUserRequest request,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var command = new UpdatePortalUserCommand(
                userId,
                id,
                request.FullName,
                request.Email,
                Enum.Parse<ClientPortalRole>(request.Role),
                request.IsActive);

            Result result = await sender.Send(command, cancellationToken);
            return result.IsSuccess ? Results.NoContent() : CustomResults.Problem(result);
        })
        .WithTags(Tags.Clients);
    }
}

public record CreatePortalUserRequest(string FullName, string Email, string PlainTextPassword, string Role);
public record UpdatePortalUserRequest(string FullName, string Email, string Role, bool IsActive);
