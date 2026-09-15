using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerFinanceJobs.Queries.GetEmployerFundingProjectionByAccountId;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace SFA.DAS.EmployerFinanceJobs.Api.Controllers;

[ApiController]
public class FundingProjectionController(IMediator mediator,
    ILogger<FundingProjectionController> logger) : ControllerBase
{
    [HttpGet]
    [Route("employer/{accountId:long}/funding-projection")]
    public async Task<IResult> GetFundingProjection([FromRoute, Required] long accountId, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await mediator.Send(new GetEmployerFundingProjectionByAccountIdQuery(accountId), cancellationToken);
            return TypedResults.Ok(result);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting employer funding projection");
            return TypedResults.StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}