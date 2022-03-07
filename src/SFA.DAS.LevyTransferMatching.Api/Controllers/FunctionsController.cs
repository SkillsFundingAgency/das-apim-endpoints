﻿using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.Api.Models.Functions;
using SFA.DAS.LevyTransferMatching.Application.Commands.ApproveApplication;
using SFA.DAS.LevyTransferMatching.Application.Commands.BackfillApplicationMatchingCriteria;
using SFA.DAS.LevyTransferMatching.Application.Commands.CreditPledge;
using SFA.DAS.LevyTransferMatching.Application.Commands.DebitApplication;
using SFA.DAS.LevyTransferMatching.Application.Commands.DebitPledge;
using SFA.DAS.LevyTransferMatching.Application.Commands.SendEmails;
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

                return new StatusCodeResult((int) result.StatusCode);
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
                    BaseUrl = request.BaseUrl,
                    ReceiverEncodedAccountId = request.ReceiverEncodedAccountId
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

                return new StatusCodeResult((int) result.StatusCode);
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

        [Route("backfill-application-matching-criteria")]
        [HttpPost]
        public async Task<IActionResult> BackfillApplicationCostingProjections()
        {
            await _mediator.Send(new BackfillApplicationMatchingCriteriaCommand());
            return Ok();
        }
    }
}
