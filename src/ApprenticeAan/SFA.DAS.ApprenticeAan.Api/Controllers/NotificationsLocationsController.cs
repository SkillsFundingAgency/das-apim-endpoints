using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Application.NotificationsLocations.Queries;

namespace SFA.DAS.ApprenticeAan.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificationsLocationsController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string searchTerm)
        {
            var result = await mediator.Send(new GetNotificationsLocationsQuery
            {
                SearchTerm = searchTerm
            });

            return Ok(result);
        }
    }
}
