using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Application.Profiles.Queries.GetProfiles;

namespace SFA.DAS.ApprenticeAan.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProfilesController : Controller
    {
        private readonly IMediator _mediator;

        public ProfilesController(IMediator mediator) => _mediator = mediator;

        /// <summary>
        /// Get list of profiles by user type
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(GetProfilesByUserTypeQueryResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProfilesByUserType([FromQuery] string userType)
        {
            var result = await _mediator.Send(new GetProfilesByUserTypeQuery(userType));

            if (result == null) return StatusCode(StatusCodes.Status500InternalServerError);

            return Ok(result);
        }
    }
}