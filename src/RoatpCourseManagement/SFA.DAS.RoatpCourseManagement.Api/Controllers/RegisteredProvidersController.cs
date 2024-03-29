﻿using System.Net;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.RoatpCourseManagement.Application.RegisteredProviders.Queries;

namespace SFA.DAS.RoatpCourseManagement.Api.Controllers
{
    [ApiController]
    public class RegisteredProvidersController : ControllerBase
    {
        private readonly ILogger<RegisteredProvidersController> _logger;
        private readonly IMediator _mediator;

        public RegisteredProvidersController(ILogger<RegisteredProvidersController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("lookup/registered-providers")]
        public async Task<IActionResult> GetRegisteredProviders()
        {
            _logger.LogInformation("Request received for all registered providers from roatp-service");
            var response = await _mediator.Send(new GetRegisteredProvidersQuery());

            if (response.StatusCode != HttpStatusCode.OK)
            {
                _logger.LogError($"registered providers not gathered, status code {response.StatusCode}, Error content:[{response.ErrorContent}]");
                return StatusCode((int)response.StatusCode, response.ErrorContent);
            }

            _logger.LogInformation($"Found {response.Body.Count} registered providers");
            return Ok(response.Body);
        }
    }
}