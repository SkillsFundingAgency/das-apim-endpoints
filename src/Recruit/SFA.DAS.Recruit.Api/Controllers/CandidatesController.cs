using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Recruit.Api.Models;
using SFA.DAS.Recruit.Application.Candidates.Commands.CandidateApplicationStatus;
using SFA.DAS.Recruit.Application.Candidates.Queries.GetCandidate;

namespace SFA.DAS.Recruit.Api.Controllers;

[ApiController]
[Route("[controller]/")]
public class CandidatesController(IMediator mediator, ILogger<CandidatesController> logger) : ControllerBase
{
    [HttpPost]
    [Route("{candidateId}/applications/{applicationId}")]
    public async Task<IActionResult> SubmitApplicationFeedback([FromRoute]Guid candidateId, [FromRoute]Guid applicationId, [FromBody]PostApplicationFeedbackRequest request)
    {
        try
        {
            await mediator.Send(new CandidateApplicationStatusCommand
            {
                Feedback = request.CandidateFeedback,
                Outcome = request.Status,
                ApplicationId = applicationId,
                CandidateId = candidateId,
                VacancyReference = request.VacancyReference,
                VacancyCity = request.VacancyCity,
                VacancyPostcode = request.VacancyPostcode,
                VacancyTitle = request.VacancyTitle,
                VacancyEmployerName = request.VacancyEmployerName
            });
            return NoContent();
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Error submitting candidate feedback : {candidateId}");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }
    [HttpGet]
    [Route("{candidateId}")]
    public async Task<IActionResult> GetCandidateDetail([FromRoute]Guid candidateId)
    {
        try
        {
            var result = await mediator.Send(new GetCandidateQuery
            {
                CandidateId = candidateId
            });
            if (result?.Candidate == null)
            {
                return NotFound();
            }
            return Ok(result.Candidate);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Error getting candidate: {candidateId}");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }
}