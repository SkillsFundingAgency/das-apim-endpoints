using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.CreateTrainingCourse;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.PostDeleteTrainingCourse;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.UpdateTrainingCourse;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.TrainingCourse;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.TrainingCourses;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
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
                YearAchieved = (int)request.YearAchieved
            });

            if (result is null) return NotFound();

            return Created($"{result.Id}", (PostTrainingCourseApiResponse)result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error posting training course for application {applicationId}", applicationId);
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetTrainingCourses([FromRoute] Guid applicationId, [FromQuery] Guid candidateId)
    {
        try
        {
            var result = await _mediator.Send(new GetTrainingCoursesQuery
            {
                CandidateId = candidateId,
                ApplicationId = applicationId
            });

            if (result is null) return NotFound();
            return Ok((Models.Applications.GetTrainingCoursesApiResponse)result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Get Training Courses : An error occurred");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpGet("{trainingCourseId}")]
    public async Task<IActionResult> GetTrainingCourse([FromRoute] Guid applicationId, [FromRoute] Guid trainingCourseId, [FromQuery] Guid candidateId)
    {
        try
        {
            var result = await _mediator.Send(new GetTrainingCourseQuery
            {
                CandidateId = candidateId,
                ApplicationId = applicationId,
                TrainingCourseId = trainingCourseId
            });

            if (result is null) return NotFound();
            return Ok((Models.Applications.GetTrainingCourseApiResponse)result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Get Training Course : An error occurred");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpPost("{trainingCourseId}")]
    public async Task<IActionResult> PostUpdateTrainingCourse([FromRoute] Guid applicationId, [FromRoute] Guid trainingCourseId, [FromBody] PostUpdateTrainingCourseApiRequest request)
    {
        try
        {
            var result = await _mediator.Send(new UpdateTrainingCourseCommand
            {
                ApplicationId = applicationId,
                CandidateId = request.CandidateId,
                TrainingCourseId = trainingCourseId,
                CourseName = request.CourseName,
                YearAchieved = request.YearAchieved
            });

            if (result is null) return NotFound();

            return Ok(result.Id);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Update Training Course : An error occurred");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpGet("{trainingCourseId}/delete")]
    public async Task<IActionResult> GetDeleteTrainingCourse([FromRoute] Guid applicationId, [FromRoute] Guid trainingCourseId, [FromQuery] Guid candidateId)
    {
        try
        {
            var result = await _mediator.Send(new GetDeleteTrainingCourseQuery
            {
                CandidateId = candidateId,
                ApplicationId = applicationId,
                TrainingCourseId = trainingCourseId
            });
            return Ok((Models.Applications.GetTrainingCourseApiResponse)result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Get Training Course : An error occurred");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpPost("{trainingCourseId}/delete")]
    public async Task<IActionResult> PostDeleteTrainingCourse([FromRoute] Guid applicationId, [FromRoute] Guid trainingCourseId, [FromBody] PostDeleteTrainingCourseRequest request)
    {
        try
        {
            var result = await _mediator.Send(new PostDeleteTrainingCourseCommand
            {
                ApplicationId = applicationId,
                TrainingCourseId = trainingCourseId,
                CandidateId = request.CandidateId
            });

            return Ok(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "DeleteJob : An error occurred");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }
}
