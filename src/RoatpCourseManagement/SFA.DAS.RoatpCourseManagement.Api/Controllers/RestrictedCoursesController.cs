using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.RoatpCourseManagement.Application.RestrictedCourses.Commands;
using SFA.DAS.RoatpCourseManagement.Application.RestrictedCourses.Queries.GetAllRestrictedCourses;
using SFA.DAS.RoatpCourseManagement.InnerApi.Responses;

namespace SFA.DAS.RoatpCourseManagement.Api.Controllers;

[ApiController]
[Route("/restricted-courses")]
public class RestrictedCoursesController(IMediator _mediator, ILogger<RestrictedCoursesController> _logger) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetRestrictedCourses([FromQuery] bool restricted)
    {
        _logger.LogInformation("Request received to get restricted courses");

        GetAllRestrictedCoursesQuery query = new(restricted);
        GetAllRestrictedCoursesResponse result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateRestrictedCourse(AddRestrictedCourseCommand command)
    {
        _logger.LogInformation("Request to create restricted course for {LarsCode}", command.LarsCode);

        await _mediator.Send(command);

        return NoContent();
    }
}
