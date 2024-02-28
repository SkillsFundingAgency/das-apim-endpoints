using System.Net;
using System.Threading.Tasks;
using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetAdditionalQuestion;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.UpdateAdditionalQuestion;

namespace SFA.DAS.FindAnApprenticeship.Api.Controllers;

[ApiController]
[Route("applications/{applicationId}/[controller]")]
public class AdditionalQuestionsController : Controller
{
    private readonly IMediator _mediator;
    private readonly ILogger<AdditionalQuestionsController> _logger;

    public AdditionalQuestionsController(IMediator mediator, ILogger<AdditionalQuestionsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromRoute] Guid applicationId, [FromQuery] Guid candidateId, [FromQuery] Guid questionId)
    {
        try
        {
            var result = await _mediator.Send(new GetAdditionalQuestionQuery
            {
                ApplicationId = applicationId,
                CandidateId = candidateId,
                QuestionId = questionId
            });

            return Ok((GetAdditionalQuestionApiResponse)result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Get Additional Question : An error occurred");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromRoute] Guid applicationId, [FromBody] PostAdditionalQuestionApiRequest request)
    {
        try
        {
            var result = await _mediator.Send(new UpdateAdditionalQuestionCommand
            {
                ApplicationId = applicationId,
                CandidateId = request.CandidateId,
                Id = request.Id,
                Answer = request.Answer
            });

            if (result is null) return NotFound();

            return Created($"{result.Id}", (PostAdditionalQuestionApiResponse)result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error posting additional question for application {applicationId}", applicationId);
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }
}
