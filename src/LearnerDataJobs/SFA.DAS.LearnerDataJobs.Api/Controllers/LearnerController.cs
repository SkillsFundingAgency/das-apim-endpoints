using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using SFA.DAS.LearnerDataJobs.Application.Commands;
using SFA.DAS.LearnerDataJobs.Application.Queries;
using SFA.DAS.LearnerDataJobs.InnerApi;

namespace SFA.DAS.LearnerDataJobs.Api.Controllers;

[Route("")]
[ApiController]
public class LearnersController(IMediator mediator, ILogger<LearnersController> logger) : ControllerBase
{
    [HttpGet("learners")]
    public async Task<IActionResult> GetAllLearners([FromQuery] int page = 1, [FromQuery] int? pagesize = 100, [FromQuery] bool excludeApproved = true)
    {
        logger.LogInformation("GetAllLearners for page {Page}, pageSize {PageSize}, excludeApproved {ExcludeApproved}", page, pagesize, excludeApproved);
        
        if (pagesize.HasValue && pagesize.Value > 1000)
        {
            return BadRequest("Page size cannot exceed 1000");
        }

        var query = new GetAllLearnersQuery()
        {
            Page = page,
            PageSize = pagesize,
            ExcludeApproved = excludeApproved
        };

        var response = await mediator.Send(query);

        return Ok(response);
    }
    
    [HttpPut]
    [Route("providers/{providerId}/learners")]
    public async Task<IActionResult> PutLearner([FromRoute] long providerId, [FromBody] LearnerDataRequest request)
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


    [HttpPatch]
    [Route("providers/{providerId}/learners/{learnerDataId}/apprenticeshipId")]
    public async Task<IActionResult> PatchLearnerDataApprenticeshipId([FromRoute] long providerId, long learnerDataId, [FromBody] LearnerDataApprenticeshipIdRequest request)
    {
        try
        {
            logger.LogTrace("Calling AssignApprenticeshipIdCommand");
            var succeeded = await mediator.Send(new AssignApprenticeshipIdCommand { ProviderId = providerId, LearnerDataId = learnerDataId, PatchRequest = request });
            if (succeeded)
            {
                return Ok();
            }
            return StatusCode((int)HttpStatusCode.FailedDependency);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "APIM error whilst attempting to assign ");
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

}