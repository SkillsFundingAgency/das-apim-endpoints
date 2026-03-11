using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LearnerData.Application.GetLearners;
using SFA.DAS.LearnerData.Application.GetShortCourseLearners;
using SFA.DAS.LearnerData.Application.UpdateLearner;
using SFA.DAS.LearnerData.Extensions;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.SharedOuterApi.Extensions;

namespace SFA.DAS.LearnerData.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class ShortCoursesController(
    IMediator mediator,
    ILogger<ShortCoursesController> logger) : ControllerBase
{
    [HttpPost]
    [Route("/providers/{ukprn}/shortCourses")]
    public async Task<IActionResult> CreateShortCourse(ShortCourseRequest request, [FromRoute] long ukprn)
    {
        try
        {
            var result = await mediator.Send(new CreateDraftShortCourseCommand
            {
                Ukprn = ukprn,
                ShortCourseRequest = request
            });

            if (result.StatusCode.IsSuccessStatusCode())
                return Accepted();
            else if(result.StatusCode == HttpStatusCode.Conflict)
                return Conflict();
            else
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Internal error occurred when creating short course");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpGet("/providers/{ukprn}/academicyears/{academicyear}/shortCourses")]
    public async Task<IActionResult> GetShortCourseLearners([FromRoute] string ukprn, [FromRoute] int academicyear, [FromQuery] int page = 1, [FromQuery] int? pagesize = 20)
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
}
