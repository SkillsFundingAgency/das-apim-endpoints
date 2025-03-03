using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AdminAan.Application.NotificationsSettings.Commands;
using SFA.DAS.AdminAan.Application.NotificationsSettings.Queries;
using SFA.DAS.AdminAan.Domain.NotificationSettings;

namespace SFA.DAS.AdminAan.Api.Controllers
{
    [Route("notification-settings")]
    [ApiController]
    public class NotificationSettingsController( IMediator mediator) : ControllerBase
    {
        [Route("")]
        [ProducesResponseType(typeof(GetNotificationsSettingsQueryResult), StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<IActionResult> Index(Guid memberId)
        {
            var result = await mediator.Send(new GetNotificationsSettingsQuery
            {
                MemberId = memberId
            });

            return Ok(result);
        }

        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost]
        public async Task<IActionResult> Index(Guid memberId, [FromBody] NotificationSettingsPostRequest body)
        {
            await mediator.Send(new UpdateNotificationSettingsCommand
            {
                MemberId = memberId,
                ReceiveNotifications = body.ReceiveNotifications
            });

            return Ok();
        }
    }
}
