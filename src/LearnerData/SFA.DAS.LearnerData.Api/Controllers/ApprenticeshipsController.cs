using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LearnerData.Application.CreateLearner;
using SFA.DAS.LearnerData.Application.Fm36;
using SFA.DAS.LearnerData.Application.GetLearners;
using SFA.DAS.LearnerData.Application.RemoveLearner;
using SFA.DAS.LearnerData.Application.UpdateLearner;
using SFA.DAS.LearnerData.Extensions;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.LearnerData.Responses;
using System.Net;
using MediatR;

namespace SFA.DAS.LearnerData.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class ApprenticeshipsController(
    IMediator mediator, 
    ILogger<ApprenticeshipsController> logger) : ControllerBase
{
    [HttpGet]
    [Route("/Learners/providers/{ukprn}/academicyears/{academicyear}/learners")]
    public async Task<IActionResult> GetLearners_Legacy([FromRoute] string ukprn, [FromRoute] int academicyear, [FromQuery] int page = 1, [FromQuery] int? pagesize = 20)
    {
        return await GetLearnersInternal(ukprn, academicyear, page, pagesize);
    }

    /// <summary>
    /// This is needed because I don't seem to be able to find from both query and route for the same parameter.
    /// The original method can be removed when SLD stop using it.  At which point, the internal method can also be moved directly into this method.
    /// </summary>
    [HttpGet]
    [Route("/providers/{ukprn}/apprenticeships/learners")]
    public async Task<IActionResult> GetLearners([FromRoute] string ukprn, [FromQuery] int academicyear, [FromQuery] int page = 1, [FromQuery] int? pagesize = 20)
    {
        return await GetLearnersInternal(ukprn, academicyear, page, pagesize);
    }

    private async Task<IActionResult> GetLearnersInternal(string ukprn, int academicyear, int page, int? pagesize)
    {
        logger.LogInformation("GetLearners for ukprn {Ukprn}, year {Year}", ukprn, academicyear);

        pagesize = pagesize.HasValue ? Math.Clamp(pagesize.Value, 1, 100) : pagesize;

        var query = new GetLearnersQuery()
        {
            Ukprn = ukprn,
            AcademicYear = academicyear,
            Page = page,
            PageSize = pagesize
        };

        var response = await mediator.Send(query);
        HttpContext.SetPageLinksInResponseHeaders(query, response);

        return Ok((GetLearnersResponse)response);
    }

    [HttpPost]
    [Route("/providers/{ukprn}/learners")]
    [Route("/providers/{ukprn}/apprenticeships")]
    public async Task<IActionResult> CreateLearningRecord([FromRoute] long ukprn, [FromBody] CreateLearnerRequest dataRequest, [FromQuery] int academicYear = 2526, [FromQuery] int collectionPeriod = 0)
    {
        try
        {
            var correlationId = Guid.NewGuid();
            await mediator.Send(new CreateLearnerCommand
            {
                CorrelationId = correlationId, 
                ReceivedOn = DateTime.Now, 
                Request = dataRequest,
                Ukprn = ukprn
            });
            return Accepted(new CorrelationResponse {CorrelationId = correlationId});
        }
        catch (Exception e)
        {
            logger.LogError(e, "Internal error occurred when processing learners list");
            return new StatusCodeResult((int) HttpStatusCode.InternalServerError);
        }
    }

    [HttpPut]
    [Route("/providers/{ukprn}/learning/{learnerKey}")]
    [Route("/providers/{ukprn}/apprenticeships/{learnerKey}")]
    public async Task<IActionResult> UpdateLearner([FromRoute] long ukprn, [FromRoute] Guid learnerKey, [FromBody] UpdateLearnerRequest request, [FromQuery] int academicyear = 2526, [FromQuery] int collectionPeriod = 0)
    {
        try
        {
            await mediator.Send(new UpdateLearnerCommand
            {
                LearnerKey = learnerKey,
                UpdateLearnerRequest = request,
                Ukprn = ukprn
            });
            return Accepted();
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Internal error occurred when updating learner {learnerKey}");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpDelete]
    [Route("/providers/{ukprn}/learning/{learningKey}")]
    [Route("/providers/{ukprn}/apprenticeships/{learningKey}")]
    public async Task<IActionResult> RemoveLearner([FromRoute] long ukprn, [FromRoute] Guid learningKey, [FromQuery] int academicyear = 2526)
    {
        logger.LogInformation(
            "RemoveLearner for provider {ukprn}, apprenticeship {learningKey}",
            ukprn,
            learningKey);

        try
        {
            var command = new RemoveLearnerCommand
            {
                LearningKey = learningKey,
                Ukprn = ukprn
            };

            await mediator.Send(command);

            return NoContent();

        }
        catch (Exception e)
        {
            logger.LogError(e, $"Internal error occurred when removing learner {learningKey}");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }

    /// <summary>
    /// Gets all earnings data.
    /// </summary>
    /// <returns>All earnings data in the format of an FM36Learner array.</returns>
    [HttpGet]
    [Route("/Learners/providers/{ukprn}/collectionPeriod/{collectionYear}/{collectionPeriod}/fm36data")]
    public async Task<IActionResult> GetFm36Learners(long ukprn, int collectionYear, byte collectionPeriod, [FromQuery] int? page, [FromQuery] int? pageSize)
    {
        return await GetFm36Data_Internal(ukprn, collectionYear, collectionPeriod, page, pageSize);
    }

    /// <summary>
    /// This is needed because I don't seem to be able to find from both query and route for the same parameter.
    /// The original method can be removed when SLD stop using it.  At which point, the internal method can also be moved directly into this method.
    /// </summary>
    [HttpGet]
    [Route("/providers/{ukprn}/fm36data")]
    public async Task<IActionResult> GetFm36Data(long ukprn, [FromQuery] int academicYear, [FromQuery] byte collectionPeriod, [FromQuery] int? page, [FromQuery] int? pageSize)
    {
        return await GetFm36Data_Internal(ukprn, academicYear, collectionPeriod, page, pageSize);
    }

    private async Task<IActionResult> GetFm36Data_Internal(long ukprn, int collectionYear, byte collectionPeriod, int? page, int? pageSize)
    {
        try
        {
            var query = new GetFm36Query(ukprn, collectionYear, collectionPeriod, page, pageSize);

            var queryResult = await mediator.Send(query);

            if (query.IsPaged)
            {
                HttpContext.SetPageLinksInResponseHeaders(query, queryResult);
                return Ok(queryResult);
            }
            else
            {
                return Ok(queryResult.Items);
            }
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error attempting to get all earnings");
            return BadRequest();
        }
    }
}