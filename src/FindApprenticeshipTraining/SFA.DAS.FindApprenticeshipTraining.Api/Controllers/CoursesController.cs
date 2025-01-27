using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FindApprenticeshipTraining.Application.Courses.GetCourses;
using System.Threading.Tasks;

namespace SFA.DAS.FindApprenticeshipTraining.Api.Controllers;

[ApiController]
[Route("[controller]/")]
public class CoursesController(IMediator _mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetCourses([FromQuery] GetCoursesQuery query)
    {
        var response = await _mediator.Send(query);
        return Ok(response);
    }
}
