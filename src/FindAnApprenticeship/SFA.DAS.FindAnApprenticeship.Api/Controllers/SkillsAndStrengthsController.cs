﻿using System.Threading.Tasks;
using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetSkillsAndStrengths;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;

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
}