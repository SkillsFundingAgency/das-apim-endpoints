using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerFeedback.Api.TaskQueue;
using SFA.DAS.EmployerFeedback.Application.Commands.SyncEmployerAccounts;
using SFA.DAS.EmployerFeedback.Extensions;
using System;
using System.Net;

namespace SFA.DAS.EmployerFeedback.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IBackgroundTaskQueue _backgroundTaskQueue;

        public AccountController(
            ILogger<AccountController> logger,
            IBackgroundTaskQueue backgroundTaskQueue)
        {
            _logger = logger;
            _backgroundTaskQueue = backgroundTaskQueue;
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
    }
}
