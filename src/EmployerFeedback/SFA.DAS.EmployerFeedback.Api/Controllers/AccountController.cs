using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;
using SFA.DAS.EmployerFeedback.Application.Commands.SyncEmployerAccounts;

namespace SFA.DAS.EmployerFeedback.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IMediator _mediator;

        public AccountController(ILogger<AccountController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateEmployerAccounts()
        {
            try
            {
                await _mediator.Send(new SyncEmployerAccountsCommand());
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error syncing employer accounts.");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
