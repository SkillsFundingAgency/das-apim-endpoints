using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;
using SFA.DAS.EmployerFeedback.Application.Commands.GenerateFeedbackSummaries;

namespace SFA.DAS.EmployerFeedback.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class DataLoadController : ControllerBase
    {
        private readonly ILogger<DataLoadController> _logger;
        private readonly IMediator _mediator;

        public DataLoadController(IMediator mediator, ILogger<DataLoadController> logger)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost("generate-feedback-summaries")]
        public async Task<IActionResult> GenerateFeedbackSummaries()
        {
            try
            {
                await _mediator.Send(new GenerateFeedbackSummariesCommand());
                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error generating feedback summaries.");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
