﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApprenticeFeedback.Application.Commands.GenerateFeedbackSummaries;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeFeedback.Api.Controllers
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
                var result = await _mediator.Send(new GenerateFeedbackSummariesCommand());
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error generating feedback summaries.");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
