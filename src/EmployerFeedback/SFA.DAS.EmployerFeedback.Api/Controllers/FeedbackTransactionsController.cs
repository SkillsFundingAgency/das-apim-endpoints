using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerFeedback.Application.Commands.SendFeedbackEmail;
using SFA.DAS.EmployerFeedback.Application.Queries.GetFeedbackTransactionUsers;
using SFA.DAS.EmployerFeedback.Application.Queries.GetFeedbackTransactionsBatch;
using SFA.DAS.EmployerFeedback.Models;
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

        [HttpPost("send")]
        public async Task<IActionResult> SendFeedbackEmail([FromBody] SendFeedbackEmailRequest request)
        {
            try
            {
                _logger.LogInformation("Received request to send feedback email for template {TemplateId}", request.TemplateId);

                await _mediator.Send(new SendFeedbackEmailCommand(request));

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending feedback email for template {TemplateId}", request.TemplateId);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("{id}/users")]
        public async Task<IActionResult> GetFeedbackTransactionUsers([FromRoute] long id)
        {
            try
            {
                _logger.LogInformation("Received request to get feedback transaction users for ID: {FeedbackTransactionId}", id);

                var result = await _mediator.Send(new GetFeedbackTransactionUsersQuery(id));

                if (result == null)
                {
                    return NoContent();
                }

                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Bad request for getting feedback transaction users {FeedbackTransactionId}: {Message}", id, ex.Message);
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting feedback transaction users for ID: {FeedbackTransactionId}", id);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}