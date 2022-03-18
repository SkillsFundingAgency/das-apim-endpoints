using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApprenticeFeedback.Application.Commands.CreateFeedbackTarget;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeFeedback.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class FeedbackTargetController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<FeedbackTargetController> _logger;

        public FeedbackTargetController(IMediator mediator, ILogger<FeedbackTargetController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<CreateFeedbackTargetResponse>> CreateFeedbackTarget(CreateFeedbackTargetCommand request)
            => await _mediator.Send(request);

    }
}
