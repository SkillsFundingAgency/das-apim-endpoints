using System;
using System.Net;
using System.Threading.Tasks;
using Azure.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindAnApprenticeship.Api.Models;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Candidate;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Users.Status;
using SFA.DAS.FindAnApprenticeship.Application.Queries.CreateAccount.Inform;

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
    [Route("create-account")]
    public async Task<IActionResult> GetCreateAccount([FromQuery] Guid candidateId)
    {
        try
        {
            var queryResponse = await _mediator.Send(new GetInformQuery
            {
                CandidateId = candidateId
            });

            return Ok(queryResponse);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error attempting to get create-account inform");
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
            var queryResponse = await _mediator.Send(new CreateCandidateCommand
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

    [HttpPost]
    [Route("{govIdentifier}/status")]
    public async Task<IActionResult> UpdateStatus(
        [FromRoute] string govIdentifier,
        [FromBody] CandidateStatus request)
    {
        try
        {
            var commandResponse = await _mediator.Send(new UpdateCandidateStatusCommand
            {
                GovUkIdentifier = govIdentifier,
                Email = request.Email,
                Status = request.Status
            });

            return Ok(commandResponse);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error attempting to update candidate status");
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}