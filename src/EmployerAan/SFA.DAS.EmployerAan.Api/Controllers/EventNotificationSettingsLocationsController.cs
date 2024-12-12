using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerAan.Application.Settings.NotificationsLocations;

namespace SFA.DAS.EmployerAan.Api.Controllers
{
    [ApiController]
    [Route("accounts/{employerAccountId}/event-notifications-settings/locations")]
    public class EventNotificationsSettingsLocationsController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get(long employerAccountId, [FromQuery] string searchTerm)
        {
            var result = await mediator.Send(new GetNotificationsLocationsQuery()
            {
                EmployerAccountId = employerAccountId,
                SearchTerm = searchTerm
            });

            return Ok(result);
        }
    }
}