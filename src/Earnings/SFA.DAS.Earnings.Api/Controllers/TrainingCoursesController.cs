using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Earnings.Api.Models;
using SFA.DAS.Earnings.Application.Training;

namespace SFA.DAS.Earnings.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class TrainingCoursesController(ILogger<TrainingCoursesController> logger, IMediator mediator) : ControllerBase
{
    [HttpGet]
    [Route("standards/{courseCode}")]
    public async Task<IActionResult> GetStandard(string courseCode)
    {
        try
        {
            var queryResult = await mediator.Send(new GetStandardQuery(courseCode));

            if (queryResult == null)
            {
                return NotFound();
            }

            var model = (GetStandardResponse)queryResult;

            return Ok(model);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error attempting to get list of standards");
            return BadRequest();
        }
    }
}
