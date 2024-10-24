

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApprenticeFeedback.Application.Commands.GenerateEmailTransaction;
using SFA.DAS.SharedOuterApi.Infrastructure;
using System.Net;
using System.Threading.Tasks;
using System;
using SFA.DAS.ApprenticeFeedback.Application.Commands.ProcessFeedbackTargetVariants;

namespace SFA.DAS.ApprenticeFeedback.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class FeedbackTargetVariantController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<FeedbackTargetVariantController> _logger;

        public FeedbackTargetVariantController(IMediator mediator, ILogger<FeedbackTargetVariantController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost("process-variants")]
        public async Task<ActionResult<NullResponse>> ProcessFeedbackTargetVariants(ProcessFeedbackTargetVariantsCommand command)
        {
            try
            {
                await _mediator.Send(command);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error processing feedback target variants");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
