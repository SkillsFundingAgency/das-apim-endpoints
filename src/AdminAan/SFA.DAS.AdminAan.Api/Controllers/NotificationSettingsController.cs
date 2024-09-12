using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AdminAan.Application.NotificationsSettings.Queries;

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
    }
}
