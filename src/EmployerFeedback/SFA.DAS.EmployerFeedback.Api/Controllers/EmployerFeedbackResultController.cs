using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerFeedback.Application.Commands.SubmitEmployerFeedback;
using SFA.DAS.EmployerFeedback.Models;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerFeedback.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class EmployerFeedbackResultController : ControllerBase
    {
        private readonly ILogger<EmployerFeedbackResultController> _logger;
        private readonly IMediator _mediator;

        public EmployerFeedbackResultController(IMediator mediator, ILogger<EmployerFeedbackResultController> logger)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost("")]
        public async Task<IActionResult> SubmitEmployerFeedback([FromBody] SubmitEmployerFeedbackRequest request)
        {
            try
            {
                var command = (SubmitEmployerFeedbackCommand)request;
                await _mediator.Send(command);
                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unhandled error submitting employer feedback.");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
