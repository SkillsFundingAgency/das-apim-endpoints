using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApprenticeFeedback.Application.Commands.CreateApprenticeFeedbackTarget;
using SFA.DAS.ApprenticeFeedback.Application.Commands.TriggerFeedbackTargetUpdate;
using SFA.DAS.ApprenticeFeedback.Application.Queries.GetFeedbackTargetsForUpdate;
using SFA.DAS.ApprenticeFeedback.Models;
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
        public async Task<IActionResult> GetApprenticeFeedbackTargetsForUpdate(int batchSize)
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
        public async Task<IActionResult> TriggerUpdate([FromBody]TriggerFeedbackTargetUpdateCommand command)
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
    }
}
