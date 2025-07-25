using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.DigitalCertificates.Application.Queries.GetUser;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.DigitalCertificates.Api.Controllers
{
    [ApiController]
    [Route("users/")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IMediator mediator, ILogger<UsersController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("{govUkIdentifier}")]
        public async Task<IActionResult> GetUser([FromRoute] string govUkIdentifier)
        {
            try
            {
                var userResult = await _mediator.Send(new GetUserQuery { GovUkIdentifier = govUkIdentifier });
                if(userResult != null)
                { 
                    return Ok(userResult.User);
                }

                return NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error attempting to retrieve user {GovUkIdentifier}", govUkIdentifier);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
