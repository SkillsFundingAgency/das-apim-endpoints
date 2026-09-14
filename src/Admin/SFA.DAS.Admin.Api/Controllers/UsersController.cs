using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Admin.Application.Commands.UnlockUser;
using SFA.DAS.Admin.Api.Models.Users;

namespace SFA.DAS.Admin.Api.Controllers
{
    [ApiController]
    [Route("users")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IMediator mediator, ILogger<UsersController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }


        [HttpPost("{userId}/unlock")]
        public async Task<IActionResult> UnlockUser([FromRoute] Guid userId, [FromBody] UnlockUserRequest request)
        {
            try
            {
                var command = (UnlockUserCommand)request;
                command.UserId = userId;

                var result = await _mediator.Send(command);

                return result == null ? BadRequest() : NoContent();
            }
            catch (ArgumentException)
            {
                return BadRequest();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error attempting to unlock user {UserId}", userId);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
