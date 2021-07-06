using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LevyTransferMatching.Api.Models;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetUserAccounts;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Api.Controllers
{
    [ApiController]
    [Route("users/{userId}")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("accounts")]
        public async Task<IActionResult> GetUserAccounts(string userId)
        {
            var queryResult = await _mediator.Send(new GetUserAccountsQuery()
            {
                UserId = userId,
            });

            return Ok(queryResult.UserAccounts.Select(x => (UserAccountDto)x));
        }
    }
}