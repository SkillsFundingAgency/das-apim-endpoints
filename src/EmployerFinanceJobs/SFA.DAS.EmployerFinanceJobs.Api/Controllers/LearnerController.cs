using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerFinanceJobs.Commands.ImportCommittedLearners;
using SFA.DAS.EmployerFinanceJobs.Commands.UpdateLearnerCost;

namespace SFA.DAS.EmployerFinanceJobs.Api.Controllers;

[ApiController]
[Route("learners")]
public class LearnerController(IMediator mediator, ILogger<LearnerController> logger) : ControllerBase
{
    [HttpPost("import")]
    [ProducesResponseType(typeof(ImportCommittedLearnersCommandResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IResult> ImportLearners()
    {
        try
        {
            var result = await mediator.Send(new ImportCommittedLearnersCommand());
            return TypedResults.Ok(result);
        }
        catch (Exception e)
        {
          logger.LogInformation("Error occurred while importing learners: {ErrorMessage}", e.Message);
          return Results.Problem(statusCode: (int)HttpStatusCode.InternalServerError);
        }
    }


    [HttpPost("update")]
    [ProducesResponseType(typeof(UpdateLearnerCostCommandResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IResult> UpdateLearner()
    {
        try
        {
            var result = await mediator.Send(new UpdateLearnerCostCommand());
            return TypedResults.Ok(result);
        }
        catch (Exception e)
        {
            logger.LogInformation("Error occurred while updating learner: {ErrorMessage}", e.Message);
            return Results.Problem(statusCode: (int)HttpStatusCode.InternalServerError);
        }
    }
}