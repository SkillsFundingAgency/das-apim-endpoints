using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SFA.DAS.EmployerFinance.Application.Queries.GetLevyProjectionsByAccountId;

namespace SFA.DAS.EmployerFinance.Api.Controllers;

[Route("finance/levy/{accountId:long}")]
[ApiController]
public class FinanceLevyController(IMediator mediator,
    ILogger<FinanceLevyController> logger) : ControllerBase
{
    [HttpGet]
    [Route("projections")]
    public async Task<IResult> GetLevyProjections(
        [FromRoute, Required] long accountId,
        [FromQuery] int months = 6)
    {
        try
        {
            var result = await mediator.Send(new GetLevyProjectionsByAccountIdQuery(accountId, months));

            return TypedResults.Ok(result);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting levy projections for account {AccountId}", accountId);
            return TypedResults.InternalServerError();
        }
    }
}