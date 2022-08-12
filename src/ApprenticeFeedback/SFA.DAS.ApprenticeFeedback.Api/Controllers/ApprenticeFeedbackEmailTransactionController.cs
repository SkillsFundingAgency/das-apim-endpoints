
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MediatR;
using SFA.DAS.ApprenticeFeedback.Application.Commands.GenerateEmailTransaction;
using SFA.DAS.ApprenticeFeedback.Application.Commands.ProcessEmailTransaction;
using SFA.DAS.ApprenticeFeedback.Application.Queries.GetFeedbackTransactionsToEmail;
using SFA.DAS.ApprenticeFeedback.Application.Queries.GetApprentice;
using SFA.DAS.ApprenticeFeedback.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Models;
using static SFA.DAS.ApprenticeFeedback.Models.Enums;
using SFA.DAS.ApprenticeFeedback.InnerApi.Requests;

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
        public async Task<ActionResult<EmailStatus>> ProcessEmailTransaction([FromRoute] long apprenticeFeedbackTransactionId, [FromBody] ApprenticeFeedbackTransaction feedbackTransaction)
        {
            GetApprenticeResult result = await _mediator.Send(new GetApprenticeQuery { ApprenticeId = feedbackTransaction.ApprenticeId });

            if (result == null)
                return NotFound(EmailStatus.Failed);

            var status = await _mediator.Send(new ProcessEmailTransactionCommand()
            {
                FeedbackTransactionId = apprenticeFeedbackTransactionId,
                ApprenticeName = result.LastName,
                ApprenticeEmailAddress = result.Email,
                IsEmailContactAllowed = result.ApprenticePreferences.Find(x => x.PreferenceId == 1)?.Status ?? false
            });

            //return Ok(EmailStatus.Successfull);
            return Ok(status);
        }

        [HttpGet]
        public async Task<ActionResult> GetFeedbackTransactionsToEmail(int batchSize)
            => Ok(await _mediator.Send(new GetFeedbackTransactionsToEmailQuery(batchSize)));
    }
}
