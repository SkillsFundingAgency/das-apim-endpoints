using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.RecruitQa.Application.Dashboard.Queries.GetQaDashboard;
using System.Net;

namespace SFA.DAS.RecruitQa.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class DashboardController(IMediator mediator, ILogger<DashboardController> logger) : ControllerBase
{
    [HttpGet]
    [Route("")]
    [ProducesResponseType(typeof(GetQaDashboardQueryResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IResult> GetQaDashboard(CancellationToken token)
    {
        try
        {
            var result = await mediator.Send(new GetQaDashboardQuery(), token);
            return TypedResults.Ok(result);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting QA dashboard");
            return Results.Problem(statusCode: (int)HttpStatusCode.InternalServerError);
        }
    }
}