using System.Threading.Tasks;
using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetSkillsAndStrengths;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.CreateSkillsAndStrengthsCommand;

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

    [HttpGet]
    public async Task<IActionResult> Get([FromRoute] Guid applicationId, [FromQuery] Guid candidateId)
    {
        try
        {
            var result = await _mediator.Send(new GetSkillsAndStrengthsQuery
            {
                ApplicationId = applicationId,
                CandidateId = candidateId
            });

            return Ok((GetSkillsAndStrengthsApiResponse)result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Get Skills and Strengths : An error occurred");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }


    [HttpPost]
    public async Task<IActionResult> Post([FromRoute] Guid applicationId, [FromBody] PostSkillsAndStrengthsApiRequest request)
    {
        try
        {
            var result = await _mediator.Send(new CreateSkillsAndStrengthsCommand
            {
                ApplicationId = applicationId,
                CandidateId = request.CandidateId,
                SkillsAndStrengths = request.SkillsAndStrengths
            });

            if (result is null) return NotFound();

            return Created($"{result.Id}", (PostSkillsAndStrengthsApiResponse)result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error posting skills and strengths for application {applicationId}", applicationId);
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }
}
