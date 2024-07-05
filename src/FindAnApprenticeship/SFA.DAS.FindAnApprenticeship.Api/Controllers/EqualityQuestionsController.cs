using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.CreateEqualityQuestionsCommand;
using System;
using System.Net;
using System.Threading.Tasks;
using SFA.DAS.FindAnApprenticeship.Application.Queries.GetEqualityQuestions;

namespace SFA.DAS.FindAnApprenticeship.Api.Controllers;

[ApiController]
public class EqualityQuestionsController(IMediator mediator, ILogger<EqualityQuestionsController> logger)
    : Controller
{
    [HttpGet]
    [Route("[controller]")]
    public async Task<IActionResult> Get([FromQuery] Guid candidateId)
    {
        try
        {
            var result = await mediator.Send(new GetEqualityQuestionsQuery
            {
                CandidateId = candidateId
            });

            return Ok((GetEqualityQuestionsApiResponse)result);
        }
        catch(Exception ex)
        {
            logger.LogError(ex, $"Error getting equality questions for candidate {candidateId}");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpPost]
    [Route("[controller]")]
    public async Task<IActionResult> Post([FromQuery] Guid candidateId, [FromBody] PostEqualityQuestionsApiRequest request)
    {
        try
        {
            var result = await mediator.Send(new UpsertAboutYouEqualityQuestionsCommand
            {
                CandidateId = candidateId,
                EthnicGroup = request.EthnicGroup,
                EthnicSubGroup = request.EthnicSubGroup,
                Sex = request.Sex,
                IsGenderIdentifySameSexAtBirth = request.IsGenderIdentifySameSexAtBirth,
                OtherEthnicSubGroupAnswer = request.OtherEthnicSubGroupAnswer,
            });

            if (result == null)
            {
                return NotFound();
            }

            return Created($"{result.Id}", (PostEqualityQuestionsApiResponse)result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Error posting equality questions for candidate {candidateId}");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }
}