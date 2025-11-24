using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LearnerDataJobs.Application.Commands;
using SFA.DAS.LearnerDataJobs.Application.Queries;
using SFA.DAS.LearnerDataJobs.InnerApi;
using System.Net;

namespace SFA.DAS.LearnerDataJobs.Api.Controllers;

[Route("")]
[ApiController]
public class LearnersController(IMediator mediator, ILogger<LearnersController> logger) : ControllerBase
{
    [HttpGet("learners")]
    public async Task<IActionResult> GetAllLearners([FromQuery] int page = 1, [FromQuery] int pagesize = 100, [FromQuery] bool excludeApproved = true)
    {
        logger.LogInformation("GetAllLearners for page {Page}, pageSize {PageSize}, excludeApproved {ExcludeApproved}", page, pagesize, excludeApproved);
        
        if (pagesize > 1000)
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

    [HttpPost]
    [Route("providers/{providerId}/learner/{learnerDataId}/apprenticeship-stop")]
    public async Task<IActionResult> ApprenticeshipStop([FromRoute] long providerId, long learnerDataId, [FromBody] ApprenticeshipStopRequest request)
    {
        try
        {
            logger.LogTrace("Calling ApprenticeshipStop");
            var command = new ApprenticeshipStopCommand(providerId, learnerDataId, request);
            logger.LogInformation($"Get learner data from API for {learnerDataId}");

            var result = await mediator.Send(command);           

            if (!result)
            {
                logger.LogInformation("Getting learner data from APi is not successful");
                return new NotFoundResult();
            }
            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "APIM error whilst attempting to assign ");
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpPost]
    [Route("providers/{providerId}/learner/{learnerDataId}/apprenticeshipstopdatechanged")]
    public async Task<IActionResult> ApprenticeshipStopDateChanged([FromRoute] long providerId, long learnerDataId, [FromBody] ApprenticeshipStopRequest request)
    {
        try
        {
            logger.LogTrace("Calling ApprenticeshipStopDateChanged");
            var command = new ApprenticeshipStopDateChangedCommand(providerId, learnerDataId, request);
            logger.LogInformation($"Get learner data from API for {learnerDataId}");

            var result = await mediator.Send(command);

            if (!result)
            {
                logger.LogInformation("Getting learner data from APi is not successful");
                return new NotFoundResult();
            }
            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "APIM error whilst attempting to assign ");
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}