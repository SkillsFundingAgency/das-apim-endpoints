using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LevyTransferMatching.Api.Models;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetAccount;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Api.Controllers
{
    [ApiController]
    [Route("accounts")]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{encodedAccountId}")]
        public async Task<IActionResult> GetAccount(string encodedAccountId)
        {
            var queryResult = await _mediator.Send(new GetAccountQuery()
            {
                EncodedAccountId = encodedAccountId,
            });

            var response = (AccountDto)queryResult.Account;

            return Ok(response);
        }
    }
}
