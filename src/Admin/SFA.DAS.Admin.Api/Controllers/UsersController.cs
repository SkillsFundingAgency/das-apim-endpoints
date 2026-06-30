using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Admin.Application.Commands.CheckUserActionByCode;
using SFA.DAS.Admin.Application.Queries.GetUserActionByCode;
using SFA.DAS.Admin.Application.Commands.UnlockUser;

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

        [HttpGet("useractions/{code}")]
        public async Task<IActionResult> GetUserActionByCode([FromRoute] string code)
        {
            try
            {
                var result = await _mediator.Send(new GetUserActionByCodeQuery { Code = code });

                return result == null ? NotFound() : Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error attempting to retrieve user action by code {Code}", code);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("useractions/{code}/all-activity")]
        public async Task<IActionResult> GetAllUserActivityByCode([FromRoute] string code)
        {
            try
            {
                var result = await _mediator.Send(new Application.Queries.GetAllUserActivityByCode.GetAllUserActivityByCodeQuery { Code = code });

                return result == null ? NotFound() : Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error attempting to retrieve all user activity by code {Code}", code);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost("useractions/{code}/search")]
        public async Task<IActionResult> CheckUserActionByCode([FromRoute] string code, [FromBody] CheckUserActionByCodeCommand command)
        {
            try
            {
                if (command == null) command = new CheckUserActionByCodeCommand();
                command.Code = string.IsNullOrEmpty(command.Code) ? code : command.Code;

                var result = await _mediator.Send(command);

                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error attempting to search user action by code {Code}", code);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost("{userId}/unlock")]
        public async Task<IActionResult> UnlockUser([FromRoute] Guid userId, [FromBody] UnlockUserCommand command)
        {
            try
            {
                if (command == null) command = new UnlockUserCommand();
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
