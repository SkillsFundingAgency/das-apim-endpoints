using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.CreateInterviewAdjustments;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetInterviewAdjustments;

namespace SFA.DAS.FindAnApprenticeship.Api.Controllers;

[ApiController]
[Route("applications/{applicationId}/[controller]")]
public class InterviewAdjustmentsController : Controller
{
    private readonly IMediator _mediator;
    private readonly ILogger<InterviewAdjustmentsController> _logger;

    public InterviewAdjustmentsController(IMediator mediator, ILogger<InterviewAdjustmentsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromRoute] Guid applicationId, [FromQuery] Guid candidateId)
    {
        try
        {
            var result = await _mediator.Send(new GetInterviewAdjustmentsQuery
            {
                ApplicationId = applicationId,
                CandidateId = candidateId
            });

            return Ok((GetInterviewAdjustmentsApiResponse)result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Get Interview Adjustments : An error occurred");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromRoute] Guid applicationId, [FromBody] PostInterviewAdjustmentsApiRequest request)
    {
        try
        {
            var result = await _mediator.Send(new UpsertInterviewAdjustmentsCommand
            {
                ApplicationId = applicationId,
                CandidateId = request.CandidateId,
                InterviewAdjustmentsDescription = request.InterviewAdjustmentsDescription,
                InterviewAdjustmentsSectionStatus = request.InterviewAdjustmentsSectionStatus
            });

            if (result == null)
            {
                return NotFound();
            }

            return Created($"{result.Id}", result.Application);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error posting interview adjustments for application {applicationId}", applicationId);
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }
}
