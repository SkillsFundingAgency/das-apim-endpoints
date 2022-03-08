using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.Application.Accounts.Queries.GetAccountQuery;
using SFA.DAS.Approvals.Application.Accounts.Queries.GetAccountUsersQuery;

namespace SFA.DAS.Approvals.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class AccountsController : ControllerBase
    {
        private readonly ILogger<AccountsController> _logger;
        private readonly IMediator _mediator;

        public AccountsController(ILogger<AccountsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{accountId}")]
        public async Task<IActionResult> Get(long accountId)
        {
            try
            {
                var result = await _mediator.Send(new GetAccountQuery {AccountId = accountId});
              
                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result);

            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting employer account");
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("{accountId}/users")]
        public async Task<IActionResult> GetUsers(long accountId)
        {
            try
            {
                var result = await _mediator.Send(new GetAccountUsersQuery { AccountId = accountId });

                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result);

            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting employer account users");
                return BadRequest();
            }
        }
    }
}