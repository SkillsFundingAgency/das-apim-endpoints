using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApprenticeFeedback.Application.Commands.GenerateEmailTransaction;
using SFA.DAS.ApprenticeFeedback.Application.Commands.ProcessEmailTransaction;
using SFA.DAS.ApprenticeFeedback.Application.Queries.GetApprentice;
using SFA.DAS.ApprenticeFeedback.Application.Queries.GetFeedbackTransactionsToEmail;
using SFA.DAS.SharedOuterApi.Models;
using System.Threading.Tasks;
using static SFA.DAS.ApprenticeFeedback.Models.Enums;

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

        [HttpPost("{apprenticeFeedbackTransactionId}")]
        public async Task<ActionResult> ProcessEmailTransaction([FromRoute] long apprenticeFeedbackTransactionId, [FromBody] ApprenticeFeedbackTransaction feedbackTransaction)
        {
            GetApprenticeResult result = await _mediator.Send(new GetApprenticeQuery { ApprenticeId = feedbackTransaction.ApprenticeId });

            if (result == null)
                return NotFound(EmailStatus.Failed);

            ProcessEmailTransactionResponse response = await _mediator.Send(new ProcessEmailTransactionCommand()
            {
                FeedbackTransactionId = apprenticeFeedbackTransactionId,
                ApprenticeName = result.FirstName,
                ApprenticeEmailAddress = result.Email,
                // If the preference is null, it's not set and we default to true until told otherwise for sending feedback emails.
                IsEmailContactAllowed = result.ApprenticePreferences.Find(x => x.PreferenceId == 1)?.Status ?? true
            });

            return Ok(new ProcessEmailTransactionResult()
            {
                FeedbackTransactionId = apprenticeFeedbackTransactionId,
                EmailStatus = response.Status
            });
        }

        [HttpGet]
        public async Task<ActionResult> GetFeedbackTransactionsToEmail(int batchSize)
            => Ok(await _mediator.Send(new GetFeedbackTransactionsToEmailQuery(batchSize)));
    }
}
