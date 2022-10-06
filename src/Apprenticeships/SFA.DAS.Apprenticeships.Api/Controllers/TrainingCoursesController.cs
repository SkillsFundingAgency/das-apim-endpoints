using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Apprenticeships.Api.Models;
using SFA.DAS.Apprenticeships.Application.TrainingCourses;

namespace SFA.DAS.Apprenticeships.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class TrainingCoursesController : ControllerBase
{
    private readonly ILogger<TrainingCoursesController> _logger;
    private readonly IMediator _mediator;

    public TrainingCoursesController(ILogger<TrainingCoursesController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpGet]
    [Route("standards/{courseCode}")]
    public async Task<IActionResult> GetStandard(string courseCode)
    {
        try
        {
            var queryResult = await _mediator.Send(new GetStandardQuery(courseCode));

            if (queryResult == null)
            {
                return NotFound();
            }

            var model = (GetStandardResponse)queryResult;

            return Ok(model);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error attempting to get list of standards");
            return BadRequest();
        }
    }
}