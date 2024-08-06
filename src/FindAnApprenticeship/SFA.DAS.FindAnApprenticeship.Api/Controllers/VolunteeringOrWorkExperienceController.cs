using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.CreateWorkExperience;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.DeleteWorkExperience;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.UpdateVolunteeringOrWorkExperience;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.VolunteeringOrWorkExperience.GetWorkExperience;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.VolunteeringOrWorkExperience.GetWorkExperiences;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Api.Controllers;

[ApiController]
[Route("applications/{applicationId}/[controller]")]
public class VolunteeringOrWorkExperienceController(
    IMediator mediator,
    ILogger<VolunteeringOrWorkExperienceController> logger)
    : Controller
{
    [HttpGet]
    public async Task<IActionResult> GetWorkExperiences([FromRoute] Guid applicationId, [FromQuery] Guid candidateId)
    {
        try
        {
            var result = await mediator.Send(new GetVolunteeringAndWorkExperiencesQuery
            {
                CandidateId = candidateId,
                ApplicationId = applicationId,
            });

            if (result is null) return NotFound();
            return Ok(result);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting work experiences : An error occurred");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpPost]
    public async Task<IActionResult> PostWorkExperience([FromRoute] Guid applicationId, [FromBody] PostWorkExperienceApiRequest request)
    {
        try
        {
            var result = await mediator.Send(new CreateWorkCommand
            {
                ApplicationId = applicationId,
                CandidateId = request.CandidateId,
                Description = request.Description,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                CompanyName = request.CompanyName
            });

            if (result == null)
            {
                return NotFound();
            }

            return Created(result.Id.ToString(), (PostWorkExperienceApiResponse)result);

        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Error posting work experience for application {applicationId}", applicationId);
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpPost]
    [Route("{id}")]
    public async Task<IActionResult> PostUpdateWorkExperience([FromRoute] Guid applicationId, [FromRoute] Guid id, [FromBody] PostUpdateVolunteeringOrWorkExperienceApiRequest request)
    {
        try
        {
            var result = await mediator.Send(new UpdateVolunteeringOrWorkExperienceCommand
            {
                ApplicationId = applicationId,
                Id = id,
                CandidateId = request.CandidateId,
                Employer = request.EmployerName,
                Description = request.Description,
                StartDate = request.StartDate,
                EndDate = request.EndDate
            });

            if (result is null) return NotFound();

            return Ok(result.Id);
        }
        catch (Exception e)
        {
            logger.LogError(e, "PostUpdateWorkExperience : An error occurred");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpGet("{id}/delete")]
    public async Task<IActionResult> GetDelete([FromRoute] Guid applicationId, [FromQuery] Guid candidateId, [FromRoute] Guid id)
    {
        try
        {
            var result = await mediator.Send(new GetVolunteeringOrWorkExperienceItemQuery
            {
                CandidateId = candidateId,
                ApplicationId = applicationId,
                Id = id
            });
            if (result.Id == Guid.Empty)
            {
                return NotFound();
            }
            return Ok((GetVolunteeringOrWorkExperienceItemApiResponse)result);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Get Volunteering or Work Experience : An error occurred");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpPost("{id}/delete")]
    public async Task<IActionResult> PostDelete([FromRoute] Guid applicationId, [FromRoute] Guid id, [FromBody] PostDeleteVolunteeringOrWorkExperienceRequest request)
    {
        try
        {
            var result = await mediator.Send(new PostDeleteVolunteeringOrWorkExperienceCommand
            {
                ApplicationId = applicationId,
                Id = id,
                CandidateId = request.CandidateId
            });

            return Ok(result);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Delete Volunteering or Work Experience : An error occurred");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }
}
