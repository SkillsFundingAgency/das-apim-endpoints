using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Admin.Application.Queries.GetUserActionByCode;

namespace SFA.DAS.Admin.Api.Controllers
{
    [ApiController]
    [Route("api/users")]
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
    }
}
