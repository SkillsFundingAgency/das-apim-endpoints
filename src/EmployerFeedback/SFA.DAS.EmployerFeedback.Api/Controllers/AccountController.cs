using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerFeedback.Api.TaskQueue;
using SFA.DAS.EmployerFeedback.Application.Commands.SyncEmployerAccounts;
using SFA.DAS.EmployerFeedback.Application.Commands.UpsertFeedbackTransaction;
using SFA.DAS.EmployerFeedback.Application.Queries.GetAccountsBatch;
using SFA.DAS.EmployerFeedback.Extensions;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerFeedback.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IBackgroundTaskQueue _backgroundTaskQueue;
        private readonly IMediator _mediator;

        public AccountController(
            ILogger<AccountController> logger,
            IBackgroundTaskQueue backgroundTaskQueue,
            IMediator mediator)
        {
            _logger = logger;
            _backgroundTaskQueue = backgroundTaskQueue;
            _mediator = mediator;
        }

        [HttpPost("update")]
        public IActionResult SyncEmployerAccounts()
        {
            var requestName = "Sync employer accounts";
            try
            {
                _logger.LogInformation($"Received request to {requestName}");
                _backgroundTaskQueue.QueueBackgroundRequest(
                    new SyncEmployerAccountsCommand(), requestName, (response, duration, log) =>
                    {
                        log.LogInformation($"Completed request to {requestName}: Request completed in {duration.ToReadableString()}");
                    });

                _logger.LogInformation($"Queued request to {requestName}");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error syncing employer accounts.");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost("{id}/feedbacktransaction")]
        public async Task<IActionResult> UpsertFeedbackTransaction([FromRoute] long id)
        {
            try
            {
                await _mediator.Send(new UpsertFeedbackTransactionCommand { AccountId = id });
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error upserting feedback transaction for account {AccountId}", id);
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAccountsBatch([FromQuery] int batchsize)
        {
            try
            {
                _logger.LogInformation($"Received request to get accounts batch with batch size: {batchsize}");

                var result = await _mediator.Send(new GetAccountsBatchQuery(batchsize));

                if (result?.AccountIds != null)
                {
                    var response = new { result.AccountIds };
                    return Ok(response);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting accounts batch.");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
