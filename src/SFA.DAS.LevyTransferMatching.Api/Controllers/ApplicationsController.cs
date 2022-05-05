using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.Api.Models.Applications;
using SFA.DAS.LevyTransferMatching.Application.Commands.SetApplicationAcceptance;
using SFA.DAS.LevyTransferMatching.Application.Commands.WithdrawApplicationAfterAcceptance;
using SFA.DAS.LevyTransferMatching.Application.Queries.Applications.GetAccepted;
using SFA.DAS.LevyTransferMatching.Application.Queries.Applications.GetApplication;
using SFA.DAS.LevyTransferMatching.Application.Queries.Applications.GetApplications;
using SFA.DAS.LevyTransferMatching.Application.Queries.Applications.GetDeclined;
using SFA.DAS.LevyTransferMatching.Application.Queries.Applications.GetWithdrawalConfirmation;
using SFA.DAS.LevyTransferMatching.Application.Queries.Applications.GetWithdrawn;

namespace SFA.DAS.LevyTransferMatching.Api.Controllers
{
    [ApiController]
    public class ApplicationsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ApplicationsController> _logger;

        public ApplicationsController(IMediator mediator, ILogger<ApplicationsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Route("accounts/{accountId}/applications")]
        public async Task<IActionResult> GetApplications(long accountId)
        {
            var result = await _mediator.Send(new GetApplicationsQuery
            {
                AccountId = accountId
            });

            return Ok(new GetApplicationsResponse
            {
                Applications = result?.Applications.Select(x => (GetApplicationsResponse.Application)x)
            });
        }

        [HttpGet]
        [Route("accounts/{accountId}/applications/{applicationId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Application(long accountId, int applicationId)
        {
            try
            {
                var result = await _mediator.Send(new GetApplicationQuery()
                {
                    AccountId = accountId,
                    ApplicationId = applicationId,
                });

                if (result != null)
                {
                    return Ok((GetApplicationResponse)result);
                }

                return NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to get {nameof(Application)} result");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("accounts/{accountId}/applications/{applicationId}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Application(long accountId, int applicationId, [FromBody] SetApplicationAcceptanceRequest request)
        {
            var successfulOperation = await _mediator.Send(new SetApplicationAcceptanceCommand
            {
                UserId = request.UserId,
                UserDisplayName = request.UserDisplayName,
                AccountId = accountId,
                ApplicationId = applicationId,
                Acceptance = request.Acceptance,
            });

            if (successfulOperation)
            {
                return NoContent();
            }

            _logger.LogInformation($"Failed to set {nameof(Application)} acceptance ({request.Acceptance}) for accountId: {accountId}, applicationId: {applicationId}");

            return BadRequest();
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [Route("/accounts/{accountId}/applications/{applicationId}/accepted")]
        public async Task<IActionResult> Accepted(int applicationId)
        {
            try
            {
                var result = await _mediator.Send(new GetAcceptedQuery()
                {
                    ApplicationId = applicationId,
                });

                if (result != null)
                {
                    return Ok((GetAcceptedResponse)result);
                }

                return NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to get {nameof(Accepted)} result");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [Route("/accounts/{accountId}/applications/{applicationId}/declined")]
        public async Task<IActionResult> Declined(int applicationId)
        {
            try
            {
                var result = await _mediator.Send(new GetDeclinedQuery()
                {
                    ApplicationId = applicationId,
                });

                if (result != null)
                {
                    return Ok((GetDeclinedResponse)result);
                }

                return NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to get {nameof(Declined)} result");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [Route("/accounts/{accountId}/applications/{applicationId}/withdrawn")]
        public async Task<IActionResult> Withdrawn(int applicationId)
        {
            try
            {
                var result = await _mediator.Send(new GetWithdrawnQuery()
                {
                    ApplicationId = applicationId,
                });

                if (result != null)
                {
                    return Ok((GetWithdrawnResponse)result);
                }

                return NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to get {nameof(Withdrawn)} result");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [Route("/accounts/{accountId}/applications/{applicationId}/withdrawal-confirmation")]
        public async Task<IActionResult> GetWithdrawalConfirmation(long accountId, int applicationId)
        {
            try
            {
                var result = await _mediator.Send(new GetWithdrawalConfirmationQuery()
                {
                    ApplicationId = applicationId
                });

                if (result != null)
                {
                    return Ok((GetWithdrawalConfirmationResponse)result);
                }

                return NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to get {nameof(GetWithdrawalConfirmation)} result");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [Route("/accounts/{accountId}/applications/{applicationId}/withdrawal-confirmation")]
        public async Task<IActionResult> WithdrawApplicationAfterAcceptance([FromBody] WithdrawApplicationAfterAcceptanceRequest request, long accountId, int applicationId)
        {
            try
            {
                var result = await _mediator.Send(new WithdrawApplicationAfterAcceptanceCommand
                {
                    AccountId = accountId,
                    ApplicationId = applicationId,
                    UserId = request.UserId,
                    UserDisplayName = request.UserDisplayName
                });

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to withdraw application after acceptance");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}