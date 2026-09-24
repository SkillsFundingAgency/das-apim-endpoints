using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerFinanceJobs.Commands.RecalculateFundingProjection;
using System.Net;

namespace SFA.DAS.EmployerFinanceJobs.Api.Controllers;

[Route("funding-projection")]
[ApiController]
public class FundingProjectionController(IMediator mediator, ILogger<FundingProjectionController> logger) : ControllerBase
{
    [HttpPost("re-calculate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(RecalculateFundingProjectionCommandResult), StatusCodes.Status200OK)]
    public async Task<IResult> RecalculateFundingProjection()
    {
        try
        {
            var result = await mediator.Send(new RecalculateFundingProjectionCommand());
            return TypedResults.Ok(result);
        }
        catch (Exception e)
        {
            logger.LogInformation("Error occurred while recalculating funding projection: {ErrorMessage}", e.Message);
            return Results.Problem(statusCode: (int)HttpStatusCode.InternalServerError);
        }
    }
}