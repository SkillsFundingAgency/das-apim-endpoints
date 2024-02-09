using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.CreateTrainingCourse;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.TrainingCourses;

namespace SFA.DAS.FindAnApprenticeship.Api.Controllers;

[ApiController]
[Route("applications/{applicationId}/[controller]")]
public class TrainingCoursesController : Controller
{
    private readonly IMediator _mediator;
    private readonly ILogger<TrainingCoursesController> _logger;

    public TrainingCoursesController(IMediator mediator, ILogger<TrainingCoursesController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> PostTrainingCourse([FromRoute] Guid applicationId, [FromBody] PostTrainingCourseApiRequest request)
    {
        try
        {
            var result = await _mediator.Send(new CreateTrainingCourseCommand
            {
                ApplicationId = applicationId,
                CandidateId = request.CandidateId,
                CourseName = request.CourseName,
                TrainingProviderName = request.TrainingProviderName,
                YearAchieved = request.YearAchieved
            });

            if (result == null)
            {
                return NotFound();
            }

            return Created(result.Id.ToString(), (PostTrainingCourseApiResponse)result);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error posting training course for application {applicationId}", applicationId);
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpGet("{candidateId}")]
    public async Task<IActionResult> GetTrainingCourses([FromRoute] Guid applicationId, [FromQuery] Guid candidateId)
    {
        try
        {
            var result = await _mediator.Send(new GetTrainingCoursesQuery
            {
                CandidateId = candidateId,
                ApplicationId = applicationId
            });
            return Ok((GetTrainingCoursesApiResponse)result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Get Training Courses : An error occurred");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpGet("")]
    public async Task<IActionResult> GetTrainingCourse([FromRoute] Guid applicationId, [FromRoute] Guid trainingCourseId, [FromQuery] Guid candidateId)
    {

    }
}
