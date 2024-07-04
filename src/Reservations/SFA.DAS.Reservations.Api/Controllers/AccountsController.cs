using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Reservations.Api.Models;
using SFA.DAS.Reservations.Application.Accounts.Queries;

namespace SFA.DAS.Reservations.Api.Controllers;

[ApiController]
[Route("[controller]/")]
public class AccountsController(IMediator mediator, ILogger<AccountsController> logger) : ControllerBase
{
    [HttpGet]
    [Route("{accountId}/users")]
    public async Task<IActionResult> GetAccountUsers(long accountId)
    {
        try
        {
            var result = await mediator.Send(new GetAccountUsersQuery(accountId));

            return Ok((GetAccountUsersApiResponse)result);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Exception caught in {ControllerName}.{ActionName}", nameof(AccountsController), nameof(GetAccountUsers));
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }
}