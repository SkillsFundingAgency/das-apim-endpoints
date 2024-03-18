using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindAnApprenticeship.Api.Models;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Candidate;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Candidate.GetCandidateDetails;

namespace SFA.DAS.FindAnApprenticeship.Api.Controllers;

[ApiController]
[Route("[controller]/")]
public class CandidatesController : ControllerBase
{
    private readonly ILogger<CandidatesController> _logger;
    private readonly IMediator _mediator;

    public CandidatesController(ILogger<CandidatesController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpGet]
    [Route("{candidateId}")]
    public async Task<IActionResult> Get([FromRoute] Guid candidateId)
    {
        try
        {
            var queryResponse = await _mediator.Send(new GetCandidateDetailsQuery
            {
                CandidateId = candidateId,
            });

            if (queryResponse is null)
                return NotFound();

            return Ok((CandidateDetailsApiResponse)queryResponse);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Get Candidate : An error occurred");
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpPut]
    [Route("{govIdentifier}")]
    public async Task<IActionResult> Index(
        [FromRoute] string govIdentifier, 
        [FromBody] CandidatesModel request)
    {
        try
        {
            var queryResponse = await _mediator.Send(new PutCandidateCommand
            {
                GovUkIdentifier = govIdentifier,
                Email = request.Email
            });

            return Ok((CandidateResponse)queryResponse);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error attempting to post candidate");
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}
