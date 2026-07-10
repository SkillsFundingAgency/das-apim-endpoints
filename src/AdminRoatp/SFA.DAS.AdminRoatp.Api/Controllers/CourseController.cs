using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AdminRoatp.Application.Queries.GetProvidersAllowedToDeliverCourse;
using SFA.DAS.AdminRoatp.Application.Queries.GetProvidersNotAllowedToDeliverCourse;
using SFA.DAS.AdminRoatp.InnerApi.Responses;

namespace SFA.DAS.AdminRoatp.Api.Controllers;

[ApiController]
[Route("/courses")]
public class CourseController(IMediator _mediator, ILogger<CourseController> _logger) : ControllerBase
{
    [HttpGet("{larsCode}/providers/allowed")]
    [ProducesResponseType(typeof(RestrictedCourseDetailsModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllowedProvidersByCourse([FromRoute] string larsCode)
    {
        _logger.LogInformation("Request received to get allowed providers by course for {LarsCode}", larsCode);

        GetProvidersAllowedToDeliverCourseQuery query = new(larsCode);
        RestrictedCourseDetailsModel result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{larsCode}/providers/not-allowed")]
    [ProducesResponseType(typeof(RestrictedCourseDetailsModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProvidersNotAllowedByCourse([FromRoute] string larsCode)
    {
        _logger.LogInformation("Request received to get providers not allowed by course for {LarsCode}", larsCode);

        GetProvidersNotAllowedToDeliverCourseQuery query = new(larsCode);
        RestrictedCourseDetailsModel result = await _mediator.Send(query);
        return Ok(result);
    }
}
