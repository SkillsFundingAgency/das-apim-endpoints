using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LevyTransferMatching.Api.Models;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetAccount;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SFA.DAS.LevyTransferMatching.Api.Controllers
{
    [ApiController]
    [Route("accounts")]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IMediator mediator, ILogger<AccountController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Route("{encodedAccountId}")]
        public async Task<IActionResult> GetAccount(string encodedAccountId)
        {
            try
            {
                var queryResult = await _mediator.Send(new GetAccountQuery()
                {
                    EncodedAccountId = encodedAccountId,
                });

                var response = (AccountDto)queryResult.Account;

                return Ok(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting account");
                return BadRequest();
            }
        }
    }
}
