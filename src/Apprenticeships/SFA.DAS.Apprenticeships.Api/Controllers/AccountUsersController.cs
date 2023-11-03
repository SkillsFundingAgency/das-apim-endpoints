using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Apprenticeships.Api.Models;
using SFA.DAS.Apprenticeships.Application.EmployerAccounts.Queries;

namespace SFA.DAS.Apprenticeships.Api.Controllers
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