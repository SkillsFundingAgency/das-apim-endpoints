using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Approvals.Api.Models;
using SFA.DAS.Approvals.Application.ProviderAccounts.Queries;

namespace SFA.DAS.Approvals.Api.Controllers;

[ApiController]
[Route("[controller]/")]
public class ProviderAccountsController(IMediator mediator) : Controller
{
    [HttpGet]
    [Route("{ukprn}")]
    public async Task<IActionResult> GetProviderStatus([FromRoute] int ukprn)
    {
        try
        {
            var result = await mediator.Send(new GetRoatpV2ProviderQuery
            {
                Ukprn = ukprn
            });

            return Ok(new ProviderAccountResponse { CanAccessService = result });

        }
        catch (Exception)
        {
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }
}