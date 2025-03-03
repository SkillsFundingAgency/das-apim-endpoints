using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Forecasting.Api.Models;
using SFA.DAS.Forecasting.Application.AccountUsers;

namespace SFA.DAS.Forecasting.Api.Controllers;

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
            logger.LogError(e, "Error getting account information for {UserId}", userId);
            return new StatusCodeResult((int) HttpStatusCode.InternalServerError);
        }
    }
}