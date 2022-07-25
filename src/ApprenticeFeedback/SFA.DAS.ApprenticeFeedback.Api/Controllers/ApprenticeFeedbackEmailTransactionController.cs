using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApprenticeFeedback.Application.Commands.GenerateEmailTransaction;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeFeedback.Api.Controllers
{
    [ApiController]
    [Route("FeedbackTransaction")]
    public class ApprenticeFeedbackEmailTransactionController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ApprenticeFeedbackEmailTransactionController> _logger;

        public ApprenticeFeedbackEmailTransactionController(IMediator mediator, ILogger<ApprenticeFeedbackEmailTransactionController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<GenerateEmailTransactionResponse>> GenerateEmailTransaction(GenerateEmailTransactionCommand request)
            => await _mediator.Send(request);

    }
}
