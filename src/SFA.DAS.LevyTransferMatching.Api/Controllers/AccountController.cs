using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LevyTransferMatching.Api.Models;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetAccount;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Net;

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
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAccount(string encodedAccountId)
        {
            _logger.LogInformation($"Getting account {encodedAccountId}");

            var queryResult = await _mediator.Send(new GetAccountQuery()
            {
                EncodedAccountId = encodedAccountId,
            });

            if (queryResult.Account == null)
            {
                _logger.LogInformation($"Account {encodedAccountId} could not be found");
                return NotFound();
            }

            var response = (AccountDto)queryResult.Account;
            
            _logger.LogInformation($"Account {encodedAccountId} has been found {response}");

            return Ok(response);
        }
    }
}