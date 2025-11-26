using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerFeedback.Application.Queries.GetFeedbackTransactionsBatch;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerFeedback.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class FeedbackTransactionsController : ControllerBase
    {
        private readonly ILogger<FeedbackTransactionsController> _logger;
        private readonly IMediator _mediator;

        public FeedbackTransactionsController(IMediator mediator, ILogger<FeedbackTransactionsController> logger)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetFeedbackTransactionsBatch([FromQuery] int batchsize)
        {
            try
            {
                _logger.LogInformation($"Received request to get feedback transactions batch with batch size: {batchsize}");

                var result = await _mediator.Send(new GetFeedbackTransactionsBatchQuery(batchsize));

                if (result?.FeedbackTransactions != null)
                {
                    var response = new { feedbackTransactions = result.FeedbackTransactions };
                    return Ok(response);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting feedback transactions batch.");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}