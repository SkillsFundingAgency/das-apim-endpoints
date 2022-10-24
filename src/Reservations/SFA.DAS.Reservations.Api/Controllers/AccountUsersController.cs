using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Reservations.Api.Models;
using SFA.DAS.Reservations.Application.AccountUsers.Queries;

namespace SFA.DAS.Reservations.Api.Controllers
{
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

                return Ok((GetUserAccountsApiResponse) result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new StatusCodeResult((int) HttpStatusCode.InternalServerError);
            }
        }
    }
}