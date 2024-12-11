using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Forecasting.Api.Models;
using SFA.DAS.Forecasting.Application.Accounts.Queries.GetAccountBalance;

namespace SFA.DAS.Forecasting.Api.Controllers;

[ApiController]
[Route("[controller]/")]
public class AccountsController(IMediator mediator, ILogger<AccountsController> logger) : ControllerBase
{
    [HttpGet]
    [Route("{accountId}/balance")]
    public async Task<IActionResult> GetAccountBalance(string accountId)
    {
        try
        {
            var result = await mediator.Send(new GetAccountBalanceQuery
            {
                AccountId = accountId
            });

            var apiResponse = (GetAccountBalanceApiResponse) result;

            if (apiResponse == null)
            {
                return NotFound();
            }

            return Ok(apiResponse);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting account balance information for {AccountId}", accountId);
            return new StatusCodeResult((int) HttpStatusCode.InternalServerError);
        }
    }
}