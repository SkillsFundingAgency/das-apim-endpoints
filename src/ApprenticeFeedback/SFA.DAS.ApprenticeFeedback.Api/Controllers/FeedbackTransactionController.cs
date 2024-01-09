using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApprenticeFeedback.Application.Commands.GenerateEmailTransaction;
using SFA.DAS.ApprenticeFeedback.Application.Commands.GenerateFeedbackSummaries;
using SFA.DAS.ApprenticeFeedback.Application.Commands.PatchApprenticeFeedbackTarget;
using SFA.DAS.ApprenticeFeedback.Application.Commands.ProcessEmailTransaction;
using SFA.DAS.ApprenticeFeedback.Application.Commands.TrackEmailTransactionClick;
using SFA.DAS.ApprenticeFeedback.Application.Queries.GetApprentice;
using SFA.DAS.ApprenticeFeedback.Application.Queries.GetFeedbackTransactionsToEmail;
using SFA.DAS.ApprenticeFeedback.Models;
using SFA.DAS.SharedOuterApi.Infrastructure;
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

        [HttpPost("generate-email-transactions")]
        public async Task<ActionResult<NullResponse>> GenerateEmailTransactions()
        {
            try
            {
                return await _mediator.Send(new GenerateEmailTransactionCommand());
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error generating email transactions");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost("{feedbackTransactionId}")]
        public async Task<ActionResult> ProcessEmailTransaction([FromRoute] long feedbackTransactionId, [FromBody] FeedbackTransaction feedbackTransaction)
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
                    FeedbackTransactionId = feedbackTransactionId,
                    EmailStatus = EmailStatus.Successful
                });
            }
            catch(Exception e)
            {
                _logger.LogError(e,$"Processing of email transaction failed for id {feedbackTransactionId}");
                return StatusCode((int)HttpStatusCode.InternalServerError, EmailStatus.Failed);
            }

            ProcessEmailTransactionResponse response = await _mediator.Send(new ProcessEmailTransactionCommand()
            {
                FeedbackTransactionId = feedbackTransactionId,
                ApprenticeName = apprentice.FirstName,
                ApprenticeEmailAddress = apprentice.Email,
                // If either preference is null, it's not set and we default to true until told otherwise for sending emails.
                IsFeedbackEmailContactAllowed = apprentice.ApprenticePreferences.Find(x => x.PreferenceId == 1)?.Status ?? true,
                IsEngagementEmailContactAllowed = apprentice.ApprenticePreferences.Find(x => x.PreferenceId == 2)?.Status ?? true
            });

            return Ok(new ProcessEmailTransactionResult()
            {
                FeedbackTransactionId = feedbackTransactionId,
                EmailStatus = response.Status
            });
        }

        [HttpPost("{feedbackTransactionId}/track-click")]
        public async Task<ActionResult> TrackClick([FromRoute] long feedbackTransactionId, [FromBody] FeedbackTransactionClick feedbackTransactionClick)
        {
            TrackEmailTransactionClickResponse response = await _mediator.Send(new TrackEmailTransactionClickCommand()
            {
                FeedbackTransactionId = feedbackTransactionId,
                ApprenticeFeedbackTargetId = feedbackTransactionClick.ApprenticeFeedbackTargetId,
                LinkName = feedbackTransactionClick.LinkName,
                LinkUrl = feedbackTransactionClick.LinkUrl,
                ClickedOn = feedbackTransactionClick.ClickedOn
            });

            return Ok(new TrackEmailTransactionClickResult()
            {
                FeedbackTransactionId = feedbackTransactionId,
                ApprenticeFeedbackTargetId = feedbackTransactionClick.ApprenticeFeedbackTargetId,
                ClickStatus = response.ClickStatus.ToString()
            });
        }

        [HttpGet]
        public async Task<ActionResult> GetFeedbackTransactionsToEmail(int batchSize)
            => Ok(await _mediator.Send(new GetFeedbackTransactionsToEmailQuery(batchSize)));
    }
}
