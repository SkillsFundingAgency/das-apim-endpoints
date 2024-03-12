using System.Threading.Tasks;
using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.CreateSkillsAndStrengthsCommand;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetExpectedSkillsAndStrengths;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetCandidateSkillsAndStrengths;

namespace SFA.DAS.FindAnApprenticeship.Api.Controllers;

[ApiController]
[Route("applications/{applicationId}/[controller]")]
public class SkillsAndStrengthsController : Controller
{
    private readonly IMediator _mediator;
    private readonly ILogger<SkillsAndStrengthsController> _logger;

    public SkillsAndStrengthsController(IMediator mediator, ILogger<SkillsAndStrengthsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet("candidate")]
    public async Task<IActionResult> GetCandidateSkillsAndStrengths([FromRoute] Guid applicationId, [FromQuery] Guid candidateId)
    {
        try
        {
            var result = await _mediator.Send(new GetCandidateSkillsAndStrengthsQuery
            {
                ApplicationId = applicationId,
                CandidateId = candidateId
            });

            return Ok((GetCandidateSkillsAndStrengthsApiResponse)result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Get Candidate Skills and Strengths : An error occurred");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpGet("expected")]
    public async Task<IActionResult> GetExpectedSkillsAndStrengths([FromRoute] Guid applicationId, [FromQuery] Guid candidateId)
    {
        try
        {
            var result = await _mediator.Send(new GetExpectedSkillsAndStrengthsQuery
            {
                ApplicationId = applicationId,
                CandidateId = candidateId
            });

            return Ok((GetExpectedSkillsAndStrengthsApiResponse)result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Get Expected Skills and Strengths : An error occurred");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }


    [HttpPost]
    public async Task<IActionResult> Post([FromRoute] Guid applicationId, [FromBody] PostSkillsAndStrengthsApiRequest request)
    {
        try
        {
            var result = await _mediator.Send(new UpsertSkillsAndStrengthsCommand
            {
                ApplicationId = applicationId,
                CandidateId = request.CandidateId,
                SkillsAndStrengths = request.SkillsAndStrengths,
                SkillsAndStrengthsSectionStatus = request.SkillsAndStrengthsSectionStatus
            });

            if (result == null)
            {
                return NotFound();
            }

            return Created($"{result.Id}", result.Application);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error posting skills and strengths for application {applicationId}", applicationId);
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }
}
