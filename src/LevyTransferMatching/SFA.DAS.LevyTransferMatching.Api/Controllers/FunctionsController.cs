﻿using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.Api.Models.Functions;
using SFA.DAS.LevyTransferMatching.Application.Commands.ApplicationCreatedForImmediateAutoApproval;
using SFA.DAS.LevyTransferMatching.Application.Commands.ApplicationWithdrawnAfterAcceptance;
using SFA.DAS.LevyTransferMatching.Application.Commands.AutoApproveApplication;
using SFA.DAS.LevyTransferMatching.Application.Commands.CreateApplication;
using SFA.DAS.LevyTransferMatching.Application.Commands.CreditPledge;
using SFA.DAS.LevyTransferMatching.Application.Commands.DebitApplication;
using SFA.DAS.LevyTransferMatching.Application.Commands.DebitPledge;
using SFA.DAS.LevyTransferMatching.Application.Commands.DeclineApprovedFunding;
using SFA.DAS.LevyTransferMatching.Application.Commands.ExpireAcceptedFunding;
using SFA.DAS.LevyTransferMatching.Application.Commands.RecalculateApplicationCostProjections;
using SFA.DAS.LevyTransferMatching.Application.Commands.RejectApplication;
using SFA.DAS.LevyTransferMatching.Application.Commands.RejectPledgeApplications;
using SFA.DAS.LevyTransferMatching.Application.Commands.SendEmails;
using SFA.DAS.LevyTransferMatching.Application.Commands.SetApplicationOutcome;
using SFA.DAS.LevyTransferMatching.Application.Queries.Functions;
using SFA.DAS.LevyTransferMatching.Extensions;

namespace SFA.DAS.LevyTransferMatching.Api.Controllers
{
    [Route("functions")]
    [ApiController]
    public class FunctionsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<FunctionsController> _logger;

        public FunctionsController(IMediator mediator, ILogger<FunctionsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [Route("application-approved")]
        [HttpPost]
        public async Task<IActionResult> ApplicationApproved(ApplicationApprovedRequest request)
        {
            try
            {
                var result = await _mediator.Send(new DebitPledgeCommand
                {
                    PledgeId = request.PledgeId,
                    Amount = request.Amount,
                    ApplicationId = request.ApplicationId
                });

                if (!result.StatusCode.IsSuccess())
                {
                    _logger.LogError($"Error attempting to Debit Pledge {result.ErrorContent}");
                }

                return new StatusCodeResult((int)result.StatusCode);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to Debit Pledge");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [Route("application-approved-receiver-notification")]
        [HttpPost]
        public async Task<IActionResult> ApplicationApprovedReceiverNotification(ApplicationApprovedReceiverNotificationRequest request)
        {
            try
            {
                await _mediator.Send(new ReceiverApplicationApprovedEmailCommand
                {
                    PledgeId = request.PledgeId,
                    ApplicationId = request.ApplicationId,
                    ReceiverId = request.ReceiverId,
                    EncodedApplicationId = request.EncodedApplicationId,
                    EncodedAccountId = request.EncodedAccountId,
                    TransfersBaseUrl = request.TransfersBaseUrl,
                    UnsubscribeUrl = request.UnsubscribeUrl
                });

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to send receiver notification email");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [Route("application-created-receiver-notification")]
        [HttpPost]
        public async Task<IActionResult> ApplicationCreatedReceiverNotification(ApplicationCreatedReceiverNotificationRequest request)
        {
            try
            {
                await _mediator.Send(new ApplicationCreatedEmailCommand
                {
                    PledgeId = request.PledgeId,
                    ApplicationId = request.ApplicationId,
                    ReceiverId = request.ReceiverId,
                    EncodedApplicationId = request.EncodedApplicationId,
                    UnsubscribeUrl = request.UnsubscribeUrl
                });

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to send receiver notification email");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [Route("application-rejected-receiver-notification")]
        [HttpPost]
        public async Task<IActionResult> ApplicationRejectedReceiverNotification(ApplicationRejectedReceiverNotificationRequest request)
        {
            try
            {
                await _mediator.Send(new ApplicationRejectedEmailCommand
                {
                    PledgeId = request.PledgeId,
                    ApplicationId = request.ApplicationId,
                    ReceiverId = request.ReceiverId,
                    BaseUrl = request.BaseUrl,
                    EncodedApplicationId = request.EncodedApplicationId
                });

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to send receiver notification email");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [Route("pledge-debit-failed")]
        [HttpPost]
        public async Task<IActionResult> PledgeDebitFailed(PledgeDebitFailedRequest request)
        {
            try
            {
                var result = await _mediator.Send(new UndoApplicationApprovalCommand
                {
                    PledgeId = request.PledgeId,
                    Amount = request.Amount,
                    ApplicationId = request.ApplicationId
                });

                if (!result.StatusCode.IsSuccess())
                {
                    _logger.LogError($"Error attempting to Undo Application Approval {result.ErrorContent}");
                }

                return new StatusCodeResult((int)result.StatusCode);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to Undo Application Approval");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [Route("debit-application")]
        [HttpPost]
        public async Task<IActionResult> DebitApplication(DebitApplicationRequest request)
        {
            try
            {
                var result = await _mediator.Send(new DebitApplicationCommand
                {
                    ApplicationId = request.ApplicationId,
                    NumberOfApprentices = request.NumberOfApprentices,
                    Amount = request.Amount
                });

                if (!result.StatusCode.IsSuccess())
                {
                    _logger.LogError($"Error attempting to Debit Application {result.ErrorContent}");
                }

                return new StatusCodeResult((int)result.StatusCode);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to Debit Application");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [Route("application-funding-declined")]
        [HttpPost]
        public async Task<IActionResult> ApplicationFundingDeclined(ApplicationFundingDeclinedRequest request)
        {
            try
            {
                var result = await _mediator.Send(new CreditPledgeCommand
                {
                    PledgeId = request.PledgeId,
                    Amount = request.Amount,
                    ApplicationId = request.ApplicationId
                });

                if (result.CreditPledgeSkipped)
                {
                    return Ok();
                }

                if (!result.StatusCode.IsSuccess())
                {
                    _logger.LogError($"Error attempting to Credit Pledge {result.ErrorContent}");
                }

                return new StatusCodeResult((int)result.StatusCode);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to Credit Pledge");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [Route("get-pending-application-email-data")]
        [HttpGet]
        public async Task<IActionResult> GetPendingApplicationEmailData()
        {
            var result = await _mediator.Send(new GetPendingApplicationEmailDataQuery());

            return Ok(result);
        }

        [Route("send-emails")]
        [HttpPost]
        public async Task<IActionResult> SendEmails(SendEmailsRequest request)
        {
            await _mediator.Send(new SendEmailsCommand()
            {
                EmailDataList = request.EmailDataList.Select(x => new SendEmailsCommand.EmailData(x.TemplateName, x.RecipientEmailAddress, x.Tokens)).ToList()
            });

            return Ok();
        }

        [Route("recalculate-application-cost-projections")]
        [HttpPost]
        public async Task<IActionResult> RecalculateApplicationCostProjections()
        {
            await _mediator.Send(new RecalculateApplicationCostProjectionsCommand());
            return Ok();
        }

        [Route("get-pledge-options-email-data")]
        [HttpGet]
        public async Task<IActionResult> GetPledgeOptionsEmailData()
        {
            var result = await _mediator.Send(new GetPledgeOptionsEmailDataQuery());

            return Ok(result);
        }

        [Route("application-withdrawn-after-acceptance")]
        [HttpPost]
        public async Task<IActionResult> ApplicationWithdrawnAfterAcceptance(ApplicationWithdrawnAfterAcceptanceRequest request)
        {
            await _mediator.Send(new ApplicationWithdrawnAfterAcceptanceCommand
            {
                ApplicationId = request.ApplicationId,
                PledgeId = request.PledgeId,
                Amount = request.Amount
            });

            return Ok();
        }

        [Route("applications-for-auto-approval")]
        [HttpGet]
        public async Task<IActionResult> ApplicationsForAutomaticApproval(int? pledgeId = null)
        {
            var result = await _mediator.Send(new ApplicationsWithAutomaticApprovalQuery { PledgeId = pledgeId });

            return Ok((GetApplicationsForAutomaticApprovalResponse)result);
        }

        [Route("approve-application")]
        [HttpPost]
        public async Task<IActionResult> ApproveApplication(ApproveApplicationRequest request)
        {
            await _mediator.Send(new AutoApproveApplicationCommand
            {
                ApplicationId = request.ApplicationId,
                PledgeId = request.PledgeId
            });

            return Ok();
        }

        [Route("application-created-immediate-auto-approval")]
        [HttpPost]
        public async Task<IActionResult> ApplicationCreatedForImmediateAutoApproval(ApplicationCreatedForImmediateAutoApprovalRequest request)
        {
            await _mediator.Send(new ApplicationCreatedForImmediateAutoApprovalCommand
            {
                ApplicationId = request.ApplicationId,
                PledgeId = request.PledgeId
            });

            return Ok();
        }

        [Route("applications-for-auto-rejection")]
        [HttpGet]
        public async Task<IActionResult> ApplicationsForAutomaticRejection()
        {
            var result = await _mediator.Send(new GetApplicationsForAutomaticRejectionQuery { });

            return Ok((GetApplicationsForAutomaticRejectionResponse)result);
        }

        [Route("reject-application")]
        [HttpPost]
        public async Task<IActionResult> RejectApplication(RejectApplicationRequest request)
        {
            await _mediator.Send(new RejectApplicationCommand
            {
                ApplicationId = request.ApplicationId,
                PledgeId = request.PledgeId
            });

            return Ok();
        }

        [Route("reject-pledge-applications")]
        [HttpPost]
        public async Task<IActionResult> RejectPledgeApplications(RejectPledgeApplicationsRequest request)
        {
            await _mediator.Send(new RejectPledgeApplicationsCommand
            {
                PledgeId = request.PledgeId
            });

            return Ok();
        }

        [Route("applications-for-auto-expire")]
        [HttpGet]
        public async Task<IActionResult> ApplicationsForAutomaticExpire()
        {
            var result = await _mediator.Send(new ApplicationsForAutomaticExpireQuery());

            return Ok((ApplicationsForAutomaticExpireResponse)result);
        }

        [Route("expire-accepted-funding")]
        [HttpPost]
        public async Task<IActionResult> ExpireAcceptedFunding(ExpireAcceptedFundingRequest request)
        {
            await _mediator.Send(new ExpireAcceptedFundingCommand
            {
                ApplicationId = request.ApplicationId
            });

            return Ok();
        }

        [Route("application-funding-expired")]
        [HttpPost]
        public async Task<IActionResult> ApplicationFundingExpired(ApplicationFundingExpiredRequest request)
        {
            try
            {
                var creditResult = await _mediator.Send(new CreditPledgeCommand
                {
                    Amount = request.Amount,
                    ApplicationId = request.ApplicationId,
                    PledgeId = request.PledgeId
                });

                if (creditResult.CreditPledgeSkipped)
                {
                    return Ok();
                }

                if (!creditResult.StatusCode.IsSuccess())
                {
                    _logger.LogError("Error attempting to Credit Pledge {ErrorContent}", creditResult.ErrorContent);
                }

                return new StatusCodeResult((int)creditResult.StatusCode);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error in ApplicationFundingExpired attempting to Credit Pledge");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [Route("applications-for-auto-decline")]
        [HttpGet]
        public async Task<IActionResult> ApplicationsForAutomaticDecline()
        {
            var result = await _mediator.Send(new ApplicationsForAutomaticDeclineQuery());

            return Ok((ApplicationsForAutomaticDeclineResponse)result);
        }

        [Route("decline-approved-funding")]
        [HttpPost]
        public async Task<IActionResult> DeclineApprovedFunding(DeclineApprovedFundingRequest request)
        {
            await _mediator.Send(new DeclineApprovedFundingCommand
            {
                ApplicationId = request.ApplicationId
            });

            return Ok();
        }
    }
}
