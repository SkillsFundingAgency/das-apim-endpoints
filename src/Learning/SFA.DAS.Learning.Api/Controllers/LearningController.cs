using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Learning.Api.Extensions;
using SFA.DAS.Learning.Api.Models;

namespace SFA.DAS.Learning.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class LearningController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<LearningController> _logger;

    public LearningController(
        ILogger<LearningController> logger,
        IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpPost]
    [Route("{apprenticeshipKey}/handleWithdrawalNotifications")]
    public async Task<ActionResult> HandleWithdrawalNotifications(Guid apprenticeshipKey, [FromBody] HandleWithdrawalNotificationsRequest request)
    {
        var apprenticeshipWithdrawnCommand = request.ToNotificationCommand(apprenticeshipKey);
        var notificationResponse = await _mediator.Send(apprenticeshipWithdrawnCommand);

        if (!notificationResponse.Success)
        {
            _logger.LogError("Error attempting to send apprenticeship withdrawn Notification(s) to the related party(ies)");
            return BadRequest();
        }
        return Ok();
    }
}
