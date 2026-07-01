using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AdminRoatp.Application.Commands.AddRestrictedCourse;
using SFA.DAS.AdminRoatp.Application.Queries.GetAllRestrictedCourses;
using SFA.DAS.AdminRoatp.InnerApi.Responses;

namespace SFA.DAS.AdminRoatp.Api.Controllers;

[ApiController]
[Route("/restricted-courses")]
public class RestrictedCoursesController(IMediator _mediator, ILogger<RestrictedCoursesController> _logger) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(GetAllRestrictedCoursesResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRestrictedCourses([FromQuery] bool restricted)
    {
        _logger.LogInformation("Request received to get restricted courses");

        GetAllRestrictedCoursesQuery query = new(restricted);
        GetAllRestrictedCoursesResponse result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> AddRestrictedCourse(AddRestrictedCourseCommand command)
    {
        _logger.LogInformation("Request to add restricted course for {LarsCode}", command.LarsCode);

        await _mediator.Send(command);

        return Created();
    }
}