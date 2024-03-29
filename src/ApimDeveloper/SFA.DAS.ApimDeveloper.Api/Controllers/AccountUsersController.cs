using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApimDeveloper.Api.ApiResponses;
using SFA.DAS.ApimDeveloper.Application.EmployerAccounts.Queries;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.ApimDeveloper.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class AccountUsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AccountUsersController> _logger;

        public AccountUsersController(IMediator mediator, ILogger<AccountUsersController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Route("{userId}/accounts")]
        public async Task<IActionResult> GetUserAccounts(string userId, [FromQuery] string email)
        {
            try
            {
                var result = await _mediator.Send(new GetAccountsQuery
                {
                    UserId = userId,
                    Email = email
                });

                return Ok((UserAccountsApiResponse)result);
            }
            catch (Exception e)
            {
                _logger.LogError(e,"Error calling GetUserAccounts");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}