using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerAan.Application.Onboarding.NotificationsLocations;

namespace SFA.DAS.EmployerAan.Api.Controllers.Onboarding
{
    [ApiController]
    [Route("accounts/{employerAccountId}/onboarding/notifications-locations")]
    public class NotificationsLocationsController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get(long employerAccountId, [FromQuery] string searchTerm)
        {
            var result = await mediator.Send(new GetNotificationsLocationsQuery
            {
                EmployerAccountId = employerAccountId,
                SearchTerm = searchTerm
            });

            return Ok(result);
        }
    }
}
