using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourseLevels;
using System.Threading.Tasks;

namespace SFA.DAS.FindApprenticeshipTraining.Api.Controllers;

[ApiController]
[Route("[controller]/")]
public class CoursesController(IMediator _mediator) : ControllerBase
{
    [HttpGet]
    [Route("levels")]
    public async Task<IActionResult> GetCourseLevels()
    {
        var response = await _mediator.Send(new GetCourseLevelsQuery());
        return Ok(response);
    }
}
