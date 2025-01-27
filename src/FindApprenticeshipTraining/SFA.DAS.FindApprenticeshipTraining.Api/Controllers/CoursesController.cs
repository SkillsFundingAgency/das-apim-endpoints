using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FindApprenticeshipTraining.Application.Courses;
using System.Threading.Tasks;

namespace SFA.DAS.FindApprenticeshipTraining.Api.Controllers;

[ApiController]
[Route("[controller]/")]
public sealed class CoursesController(IMediator _mediator) : ControllerBase
{
    [HttpGet]
    [Route("routes")]
    public async Task<IActionResult> GetCourseRoutes()
    {
        var result = await _mediator.Send(new GetCourseRoutesQuery());
        return Ok(result);
    }
}
