using Application.Clients.Commands.CreatePortalUser;
using Application.Clients.Commands.RegisterClient;
using Application.Clients.Commands.UpdateClient;
using Application.Clients.Commands.UpdatePortalUser;
using Application.Clients.Queries.GetClientById;
using Application.Clients.Queries.GetClients;
using Application.Clients.Queries.GetPortalUsers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/clients")]
public class ClientsController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetClients(
        [FromQuery] string? searchTerm,
        [FromQuery] bool? isActive,
        CancellationToken cancellationToken)
    {
        var query = new GetClientsQuery(searchTerm, isActive);
        SharedKernel.Result<IReadOnlyList<ClientResponse>> result = await sender.Send(query, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetClientById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetClientByIdQuery(id);
        SharedKernel.Result<ClientResponse> result = await sender.Send(query, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
    }

    [HttpPost]
    public async Task<IActionResult> RegisterClient([FromBody] RegisterClientCommand command, CancellationToken cancellationToken)
    {
        SharedKernel.Result<Guid> result = await sender.Send(command, cancellationToken);
        return result.IsSuccess ? CreatedAtAction(nameof(GetClientById), new { id = result.Value }, result.Value) : BadRequest(result.Error);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateClient(Guid id, [FromBody] UpdateClientCommand command, CancellationToken cancellationToken)
    {
        if (id != command.ClientId)
        {
            return BadRequest();
        }

        SharedKernel.Result result = await sender.Send(command, cancellationToken);
        return result.IsSuccess ? NoContent() : BadRequest(result.Error);
    }

    // Portal Users Endpoints nested under clients

    [HttpGet("{clientId:guid}/portal-users")]
    public async Task<IActionResult> GetPortalUsers(Guid clientId, [FromQuery] bool? isActive, CancellationToken cancellationToken)
    {
        var query = new GetPortalUsersQuery(clientId, isActive);
        SharedKernel.Result<IReadOnlyList<ClientPortalUserResponse>> result = await sender.Send(query, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [HttpPost("{clientId:guid}/portal-users")]
    public async Task<IActionResult> CreatePortalUser(Guid clientId, [FromBody] CreatePortalUserCommand command, CancellationToken cancellationToken)
    {
        if (clientId != command.ClientId)
        {
            return BadRequest();
        }

        SharedKernel.Result<Guid> result = await sender.Send(command, cancellationToken);
        // Note: For simplicity returning OK with ID, typical pattern could be CreatedAtAction if we had GetPortalUserById
        return result.IsSuccess ? Ok(new { Id = result.Value }) : BadRequest(result.Error);
    }

    [HttpPut("{clientId:guid}/portal-users/{userId:guid}")]
    public async Task<IActionResult> UpdatePortalUser(Guid clientId, Guid userId, [FromBody] UpdatePortalUserCommand command, CancellationToken cancellationToken)
    {
        if (clientId != command.ClientId || userId != command.UserId)
        {
            return BadRequest();
        }

        SharedKernel.Result result = await sender.Send(command, cancellationToken);
        return result.IsSuccess ? NoContent() : BadRequest(result.Error);
    }
}
