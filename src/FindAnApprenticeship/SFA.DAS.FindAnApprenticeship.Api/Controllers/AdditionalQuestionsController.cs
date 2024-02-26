using System.Net;
using System.Threading.Tasks;
using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetAdditionalQuestionTwo;

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

    [HttpGet("question-two")]
    public async Task<IActionResult> GetQuestionTwo([FromRoute] Guid applicationId, [FromQuery] Guid candidateId)
    {
        try
        {
            var result = await _mediator.Send(new GetAdditionalQuestionTwoQuery
            {
                ApplicationId = applicationId,
                CandidateId = candidateId
            });

            return Ok((GetAdditionalQuestionTwoApiResponse)result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Get Additional Question Two : An error occurred");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }
}
