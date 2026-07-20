using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LearnerData.Application.CreateShortCourseLearning;
using SFA.DAS.LearnerData.Application.GetShortCourseEarnings;
using SFA.DAS.LearnerData.Application.GetShortCourseLearners;
using SFA.DAS.LearnerData.Application.RemoveShortCourse;
using SFA.DAS.LearnerData.Application.UpdateShortCourse;
using SFA.DAS.LearnerData.Extensions;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.LearnerData.Services.ShortCourses;

namespace SFA.DAS.LearnerData.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class ShortCoursesController(
    IMediator mediator,
    ILogger<ShortCoursesController> logger) : ControllerBase
{
    [HttpPost]
    [Route("/providers/{ukprn}/shortCourses")]
    public async Task<IActionResult> CreateShortCourse(ShortCourseRequest request, [FromRoute] long ukprn, [FromQuery] int academicYear = 2526, [FromQuery] int collectionPeriod = 0)
    {
        try
        {
            var result = await mediator.Send(new CreateDraftShortCourseCommand
            {
                Ukprn = ukprn,
                AcademicYear = academicYear,
                ShortCourseRequest = request
            });

            return Accepted(new { result.CorrelationId });
        }
        catch (InvalidCourseException e)
        {
            logger.LogError(e, "Invalid course code when creating short course");
            return new StatusCodeResult((int)HttpStatusCode.UnprocessableEntity);
        }
        catch (CoursesApiUnavailableException e)
        {
            logger.LogError(e, "Courses API unavailable when creating short course");
            return new StatusCodeResult((int)HttpStatusCode.ServiceUnavailable);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Internal error occurred when creating short course");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpGet]
    [Route("/providers/{ukprn}/academicyears/{academicyear}/shortCourses")]
    public async Task<IActionResult> GetShortCourseLearners_Legacy([FromRoute] string ukprn, [FromRoute] int academicyear, [FromQuery] int page = 1, [FromQuery] int? pagesize = 20)
    {
        return await GetShortCourseLearner_Internal(ukprn, academicyear, page, pagesize);

    }

    /// <summary>
    /// This is needed because I don't seem to be able to find from both query and route for the same parameter.
    /// The original method can be removed when SLD stop using it.  At which point, the internal method can also be moved directly into this method.
    /// </summary>
    [HttpGet]
    [Route("/providers/{ukprn}/shortCourses/learners")]
    public async Task<IActionResult> GetShortCourseLearners([FromRoute] string ukprn, [FromQuery] int academicyear, [FromQuery] int page = 1, [FromQuery] int? pagesize = 20)
    {
        return await GetShortCourseLearner_Internal(ukprn, academicyear, page, pagesize);

    }

    private async Task<IActionResult> GetShortCourseLearner_Internal(string ukprn, int academicyear, int page, int? pagesize)
    {
        logger.LogInformation("GetShortCourseLearners for ukprn {Ukprn}, year {Year}", ukprn, academicyear);

        pagesize = pagesize.HasValue ? Math.Clamp(pagesize.Value, 1, 100) : pagesize;

        var query = new GetShortCourseLearnersQuery()
        {
            Ukprn = ukprn,
            AcademicYear = academicyear,
            Page = page,
            PageSize = pagesize
        };

        var response = await mediator.Send(query);
        HttpContext.SetPageLinksInResponseHeaders(query, response);

        return Ok((GetShortCourseLearnersResponse)response);
    }

    // This is the short course equivalent of FM36
    [HttpGet]
    [Route("/providers/{ukprn}/collectionPeriods/{collectionYear}/{collectionPeriod}/shortCourses")]
    public async Task<IActionResult> GetShortCourseEarnings_Legacy([FromRoute] long ukprn, [FromRoute] int collectionYear, [FromRoute] byte collectionPeriod, [FromQuery] int page = 1, [FromQuery] int? pagesize = 20)
    {
        return await GetShortCourseEarnings_Internal(ukprn, collectionYear, collectionPeriod, page, pagesize);
    }

    /// <summary>
    /// This is needed because I don't seem to be able to find from both query and route for the same parameter.
    /// The original method can be removed when SLD stop using it.  At which point, the internal method can also be moved directly into this method.
    /// </summary>
    [HttpGet]
    [Route("/providers/{ukprn}/shortCourses/earnings")]
    public async Task<IActionResult> GetShortCourseEarnings([FromRoute] long ukprn, [FromQuery] int academicYear, [FromQuery] byte collectionPeriod, [FromQuery] int page = 1, [FromQuery] int? pagesize = 20)
    {
        return await GetShortCourseEarnings_Internal(ukprn, academicYear, collectionPeriod, page, pagesize);
    }

    private async Task<IActionResult> GetShortCourseEarnings_Internal(long ukprn, int collectionYear, byte collectionPeriod, int page, int? pagesize)
    {
        logger.LogInformation("GetShortCourseEarnings for ukprn {Ukprn}, year {Year} and period {period}", ukprn, collectionYear, collectionPeriod);

        pagesize = pagesize.HasValue ? Math.Clamp(pagesize.Value, 1, 100) : pagesize;

        var query = new GetShortCourseEarningsQuery(ukprn, collectionYear, collectionPeriod, page, pagesize);

        var result = await mediator.Send(query);
        HttpContext.SetPageLinksInResponseHeaders(query, result);

        return Ok(result);
    }

    [HttpDelete("/providers/{ukprn}/shortCourses/{learnerKey}")]
    public async Task<IActionResult> RemoveShortCourse([FromRoute] long ukprn, [FromRoute] Guid learnerKey, [FromQuery] int academicYear = 2526)
    {
        try
        {
            await mediator.Send(new RemoveShortCourseCommand
            {
                Ukprn = ukprn,
                LearnerKey = learnerKey,
                AcademicYear = academicYear
            });

            return Accepted();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Internal error occurred when deleting short course");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpPut("/providers/{ukprn}/shortCourses/{learnerKey}")]
    public async Task<IActionResult> UpdateShortCourseLearning(Guid learnerKey, ShortCourseRequest request, long ukprn, [FromQuery] int academicYear = 2526, [FromQuery] int collectionPeriod = 0)
    {
        try
        {
            await mediator.Send(new UpdateShortCourseLearningCommand
            {
                LearnerKey = learnerKey,
                Ukprn = ukprn,
                AcademicYear = academicYear,
                Request = request
            });
        }
        catch (Exception e)
        {
            logger.LogError(e, "Internal error occurred when updating short course learning");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }

        return Accepted();
    }
}
