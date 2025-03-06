using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerFinance.Api.Models;
using SFA.DAS.EmployerFinance.Application.Queries.Transfers.GetPledges;

namespace SFA.DAS.EmployerFinance.Api.Controllers;

[ApiController]
[Route("[controller]/")]
public class PledgesController(IMediator mediator, ILogger<PledgesController> logger) : ControllerBase
{
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetPledges([Required]long accountId)
    {
        try
        {
            var response = await mediator.Send(new GetPledgesQuery()
            {
                AccountId = accountId,
            });

            var model = (GetPledgesResponse)response;

            return Ok(model);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting pledges");
            return BadRequest();
        }
    }
}