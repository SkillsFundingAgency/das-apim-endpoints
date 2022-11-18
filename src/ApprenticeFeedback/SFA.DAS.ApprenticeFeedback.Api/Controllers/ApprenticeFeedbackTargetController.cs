using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApprenticeFeedback.Application.Commands.CreateApprenticeFeedbackTarget;
using SFA.DAS.ApprenticeFeedback.Application.Commands.TriggerFeedbackTargetUpdate;
using SFA.DAS.ApprenticeFeedback.Application.Queries.GetApprenticeFeedbackTargets;
using SFA.DAS.ApprenticeFeedback.Application.Queries.GetExitSurvey;
using SFA.DAS.ApprenticeFeedback.Application.Queries.GetFeedbackTargetsForUpdate;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeFeedback.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class ApprenticeFeedbackTargetController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ApprenticeFeedbackTargetController> _logger;

        public ApprenticeFeedbackTargetController(IMediator mediator, ILogger<ApprenticeFeedbackTargetController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<CreateApprenticeFeedbackTargetResponse>> CreateFeedbackTarget(CreateApprenticeFeedbackTargetCommand request)
            => await _mediator.Send(request);

        [HttpGet("requiresupdate")]
        public async Task<IActionResult> GetFeedbackTargetsForUpdate(int batchSize)
        {
            try
            {
                var result = await _mediator.Send(new GetFeedbackTargetsForUpdateQuery() { BatchSize = batchSize });
                return Ok(result.FeedbackTargetsForUpdate);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting feedback targets for update.");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateFeedbackTarget([FromBody]TriggerFeedbackTargetUpdateCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error triggering feedback target update.");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("{apprenticeId}")]
        public async Task<IActionResult> GetAllForApprentice(Guid apprenticeId)
        {
            try
            {
                var result = await _mediator.Send(new GetApprenticeFeedbackTargetsQuery { ApprenticeId = apprenticeId });
                return Ok(result.FeedbackTargets);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to retrieve apprentice feedback targets for ApprenticeId: {apprenticeId}");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("{id}/exitsurvey")]
        public async Task<IActionResult> GetExitSurveyForApprenticeFeedbackTarget(Guid id)
        {
            try
            {
                var result = await _mediator.Send(new GetExitSurveyForApprenticeFeedbackTargetQuery { ApprenticeFeedbackTargetId = id });
                return Ok(result.ExitSurvey);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to retrieve exit survey for apprentice feedback target id: {id}");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
