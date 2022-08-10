
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MediatR;
using SFA.DAS.ApprenticeFeedback.Application.Commands.GenerateEmailTransaction;
using SFA.DAS.ApprenticeFeedback.Application.Queries.GetFeedbackTransactionsToEmail;
using System.Collections.Generic;
using SFA.DAS.ApprenticeFeedback.InnerApi.Responses;

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

        [HttpGet]
        public async Task<ActionResult/*<IEnumerable<GetFeedbackTransactionsToEmailResponse>>*/> GetFeedbackTransactionsToEmail(int batchSize)
            => Ok(await _mediator.Send(new GetFeedbackTransactionsToEmailQuery(batchSize)));
    }
}
