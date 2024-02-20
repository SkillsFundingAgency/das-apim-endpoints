using System.Net;
using System.Threading.Tasks;
using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.VolunteeringOrWorkExperience.DeleteVolunteeringOrWorkExperience;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.DeleteVolunteeringOrWorkExperience;

namespace SFA.DAS.FindAnApprenticeship.Api.Controllers;

[ApiController]
[Route("applications/{applicationId}/[controller]")]
public class VolunteeringOrWorkExperienceController : Controller
{
    private readonly IMediator _mediator;
    private readonly ILogger<VolunteeringOrWorkExperienceController> _logger;

    public VolunteeringOrWorkExperienceController(IMediator mediator, ILogger<VolunteeringOrWorkExperienceController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet("{id}/delete")]
    public async Task<IActionResult> GetDelete([FromRoute] Guid applicationId, [FromQuery] Guid candidateId, [FromRoute] Guid id)
    {
        try
        {
            var result = await _mediator.Send(new GetVolunteeringOrWorkExperienceItemQuery
            {
                CandidateId = candidateId,
                ApplicationId = applicationId,
                Id = id
            });
            return Ok((GetVolunteeringOrWorkExperienceItemApiResponse)result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Get Volunteering or Work Experience : An error occurred");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpPost("{id}/delete")]
    public async Task<IActionResult> PostDelete([FromRoute] Guid applicationId, [FromRoute] Guid id, [FromBody] PostDeleteVolunteeringOrWorkExperienceRequest request)
    {
        try
        {
            var result = await _mediator.Send(new PostDeleteVolunteeringOrWorkExperienceCommand
            {
                ApplicationId = applicationId,
                Id = id,
                CandidateId = request.CandidateId
            });

            return Ok(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Delete Volunteering or Work Experience : An error occurred");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }
}
