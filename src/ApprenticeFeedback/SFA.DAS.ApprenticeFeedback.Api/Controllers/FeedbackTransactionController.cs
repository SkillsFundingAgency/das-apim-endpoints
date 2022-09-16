using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApprenticeFeedback.Application.Commands.GenerateEmailTransaction;
using SFA.DAS.ApprenticeFeedback.Application.Commands.PatchApprenticeFeedbackTarget;
using SFA.DAS.ApprenticeFeedback.Application.Commands.ProcessEmailTransaction;
using SFA.DAS.ApprenticeFeedback.Application.Queries.GetApprentice;
using SFA.DAS.ApprenticeFeedback.Application.Queries.GetFeedbackTransactionsToEmail;
using SFA.DAS.ApprenticeFeedback.Models;
using SFA.DAS.SharedOuterApi.Models;
using System;
using System.Net;
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
            GetApprenticeResult apprentice = null;
            try
            {
                apprentice = await _mediator.Send(new GetApprenticeQuery { ApprenticeId = feedbackTransaction.ApprenticeId });
            }
            catch (ApprenticeNotFoundException)
            {
                // close off transaction status of 3 for complete
                await _mediator.Send(new PatchApprenticeFeedbackTargetCommand { ApprenticeFeedbackTargetId = feedbackTransaction.ApprenticeFeedbackTargetId, Status = (int)FeedbackTargetStatus.Complete, FeedbackEligibilityStatus = (int)FeedbackEligibility.Deny_Complete });

                // Returning success as no email sent and it didn't fail we've just closed it off.
                return Ok(new ProcessEmailTransactionResult()
                {
                    FeedbackTransactionId = apprenticeFeedbackTransactionId,
                    EmailStatus = EmailStatus.Successful
                });
            }
            catch(Exception e)
            {
                _logger.LogError(e,$"Processing of email transaction failed for id {apprenticeFeedbackTransactionId}");
                return StatusCode((int)HttpStatusCode.InternalServerError, EmailStatus.Failed);
            }

            ProcessEmailTransactionResponse response = await _mediator.Send(new ProcessEmailTransactionCommand()
            {
                FeedbackTransactionId = apprenticeFeedbackTransactionId,
                ApprenticeName = apprentice.FirstName,
                ApprenticeEmailAddress = apprentice.Email,
                // If the preference is null, it's not set and we default to true until told otherwise for sending feedback emails.
                IsEmailContactAllowed = apprentice.ApprenticePreferences.Find(x => x.PreferenceId == 1)?.Status ?? true
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
