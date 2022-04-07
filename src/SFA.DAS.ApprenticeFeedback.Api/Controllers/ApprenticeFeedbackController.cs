using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApprenticeFeedback.Application.Commands.CreateApprenticeFeedback;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeFeedback.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class ApprenticeFeedbackController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ApprenticeFeedbackController> _logger;

        public ApprenticeFeedbackController(IMediator mediator, ILogger<ApprenticeFeedbackController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<CreateApprenticeFeedbackResponse>> CreateFeedbackTarget(CreateApprenticeFeedbackCommand request)
            => await _mediator.Send(request);

    }
}
