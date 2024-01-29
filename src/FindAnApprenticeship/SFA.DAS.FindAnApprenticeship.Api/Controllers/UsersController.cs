using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Users;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Api.Controllers
{
    public class UsersController : Controller
    {
        private readonly ILogger<LocationsController> _logger;
        private readonly IMediator _mediator;

        public UsersController(ILogger<LocationsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPut]
        [Route ("add-details")]
        public async Task <IActionResult> AddDetails ([FromQuery] string FirstName, [FromQuery] string LastName, [FromQuery] string GovUkIdentifier)
        {
            try
            {
                var result = await _mediator.Send(new AddDetailsCommand { FirstName = FirstName, LastName = LastName, GovUkIdentifier = GovUkIdentifier });

                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error saving details");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
