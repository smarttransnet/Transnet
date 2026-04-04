using System.Security.Claims;
using Application.Trips.GetPortalJobs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/client-portal/jobs")]
public class ClientPortalJobsController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetJobs(CancellationToken cancellationToken)
    {
        string? clientIdClaim = User.FindFirstValue("clientId");

        if (string.IsNullOrEmpty(clientIdClaim) || !Guid.TryParse(clientIdClaim, out Guid clientId))
        {
            return Unauthorized("Client identifier not found in token.");
        }

        var query = new GetPortalJobsQuery(clientId);
        var result = await sender.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
}
