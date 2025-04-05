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
    [ApiController]
    [Route("[controller]/")]
    public class AccountUsersController(IMediator mediator, ILogger<AccountUsersController> logger) : ControllerBase
    {
        [HttpGet]
        [Route("{userId}/accounts")]
        public async Task<IActionResult> GetUserAccounts(string userId, [FromQuery] string email)
        {
            try
            {
                var result = await mediator.Send(new GetAccountsQuery
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