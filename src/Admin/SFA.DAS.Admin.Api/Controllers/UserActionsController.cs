using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Admin.Api.Models.UserActions;
using SFA.DAS.Admin.Application.Commands.CheckUserActionByCode;
using SFA.DAS.Admin.Application.Queries.GetUserActionByCode;

namespace SFA.DAS.Admin.Api.Controllers
{
    [ApiController]
    [Route("user-actions")]
    public class UserActionsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UserActionsController> _logger;

        public UserActionsController(IMediator mediator, ILogger<UserActionsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("{code}")]
        public async Task<IActionResult> GetUserActionByCode([FromRoute] string code)
        {
            try
            {
                var result = await _mediator.Send(new GetUserActionByCodeQuery { Code = code });

                if (result == null) return NotFound();

                var response = (GetUserActionByCodeResponse)result;

                return Ok(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error attempting to retrieve user action by code {Code}", code);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("{code}/all-activity")]
        public async Task<IActionResult> GetAllUserActivityByCode([FromRoute] string code)
        {
            try
            {
                var result = await _mediator.Send(new Application.Queries.GetAllUserActivityByCode.GetAllUserActivityByCodeQuery { Code = code });

                if (result == null) return NotFound();

                var response = (GetAllUserActivityByCodeResponse)result;

                return Ok(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error attempting to retrieve all user activity by code {Code}", code);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost("{code}/search")]
        public async Task<IActionResult> CheckUserActionByCode([FromRoute] string code, [FromBody] CheckUserActionByCodeRequest request)
        {
            try
            {
            var command = (CheckUserActionByCodeCommand)request;
                command.Code = code;

                var result = await _mediator.Send(command);

                var response = (CheckUserActionByCodeResponse)result;

                return Ok(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error attempting to search user action by code {Code}", code);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
