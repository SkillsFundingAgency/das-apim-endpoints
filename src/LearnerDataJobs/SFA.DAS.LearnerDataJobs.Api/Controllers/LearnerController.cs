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
            var created = await mediator.Send(new AddLearnerDataCommand {LearnerData = request});
            if (created)
            {
                return Created();
            }
            return StatusCode((int)HttpStatusCode.FailedDependency);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "APIM error whilst attempting to add new learner data");
            return StatusCode((int) HttpStatusCode.InternalServerError);
        }
    }
}