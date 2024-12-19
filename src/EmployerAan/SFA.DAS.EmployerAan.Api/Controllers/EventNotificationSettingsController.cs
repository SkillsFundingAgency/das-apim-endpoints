using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerAan.Application.Settings.Commands;
using SFA.DAS.EmployerAan.Application.Settings.NotificationsLocations;
using SFA.DAS.EmployerAan.Models.ApiRequests.Settings;

namespace SFA.DAS.EmployerAan.Api.Controllers
{
    [ApiController]
    [Route("accounts/{employerAccountId}/event-notifications-settings/locations")]
    public class EventNotificationsSettingsController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get(long employerAccountId, [FromQuery] string? searchTerm, [FromQuery] Guid memberId)
        {
            var result = await mediator.Send(new GetNotificationsLocationsQuery()
            {
                EmployerAccountId = employerAccountId,
                MemberId = memberId,
                SearchTerm = searchTerm
            });

            return Ok(result);
        }
    }
}