using Application.Reminders.Commands.SendReminder;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/reminders")]
public class RemindersController(ISender sender) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> SendReminder([FromBody] SendReminderCommand command, CancellationToken cancellationToken)
    {
        SharedKernel.Result<Guid> result = await sender.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(new { ReminderLogId = result.Value }) : BadRequest(result.Error);
    }
}
