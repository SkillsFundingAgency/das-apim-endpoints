using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AdminRoatp.Application.Queries.GetProviderAvailableCourses;
using SFA.DAS.AdminRoatp.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.InnerApi;

namespace SFA.DAS.AdminRoatp.Api.Controllers;

[ApiController]
[Route("providers/{ukprn}")]
public class ProviderAvailableCoursesController(IMediator _mediator, ILogger<ProviderAvailableCoursesController> _logger) : ControllerBase
{
    [HttpGet("courses-tobe-allowed")]
    [ProducesResponseType(typeof(RestrictedCourseDetailsModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProviderAvailableCourses([FromRoute] int ukprn, [FromQuery] CourseType? courseType = null)
    {
        _logger.LogInformation("Request received to GetProviderAvailableCourses for {Ukprn} and {CourseType}", ukprn, courseType);

        GetProviderAvailableCoursesQuery query = new() { Ukprn = ukprn, CourseType = courseType };
        GetProviderAvailableCoursesQueryResult result = await _mediator.Send(query);
        return Ok(result);
    }
}
