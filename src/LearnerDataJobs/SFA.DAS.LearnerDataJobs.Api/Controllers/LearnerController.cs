using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using SFA.DAS.LearnerDataJobs.Application.Commands;
using SFA.DAS.LearnerDataJobs.InnerApi;

namespace SFA.DAS.LearnerDataJobs.Api.Controllers;

[ApiController]
[Route("learners")]

public class LearnersController(IMediator mediator, ILogger<LearnersController> logger) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> PostLearner([FromBody] LearnerDataRequest request)
    {
        try
        {
            logger.LogTrace("Calling AddLearnerCommand");
            var result = await mediator.Send(new AddLearnerDataCommand {LearnerData = request});
            if ((int) result >= 200 && (int) result < 300)
            {
                return Created();
            }
            logger.LogInformation("Calling AddLearnerDataCommand returned a status of {0}", result);
            return StatusCode((int)HttpStatusCode.FailedDependency);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "APIM error whilst attempting to add new learner data");
            return StatusCode((int) HttpStatusCode.InternalServerError);
        }
    }
}