using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateOrUpdateUser;
using SFA.DAS.DigitalCertificates.Application.Queries.GetUser;
using SFA.DAS.DigitalCertificates.Models;
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

        [HttpPost("identity")]
        public async Task<IActionResult> CreateOrUpdateUser([FromBody] CreateOrUpdateUserRequest request)
        {
            try
            {
                var command = new CreateOrUpdateUserCommand
                {
                    GovUkIdentifier = request.GovUkIdentifier,
                    EmailAddress = request.EmailAddress,
                    PhoneNumber = request.PhoneNumber,
                    Names = request.Names,
                    DateOfBirth = request.DateOfBirth
                };

                var result = await _mediator.Send(command);
                return Ok(result.UserId);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error attempting to create or update user.");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
