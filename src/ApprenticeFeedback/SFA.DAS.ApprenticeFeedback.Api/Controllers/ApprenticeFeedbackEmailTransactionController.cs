using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApprenticeFeedback.Application.Commands.GenerateEmailTransaction;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeFeedback.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class FeedbackTransactionController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<FeedbackTransactionController> _logger;

        public FeedbackTransactionController(IMediator mediator, ILogger<FeedbackTransactionController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<GenerateEmailTransactionResponse>> GenerateEmailTransaction()
            => await _mediator.Send(new GenerateEmailTransactionCommand());

    }
}
