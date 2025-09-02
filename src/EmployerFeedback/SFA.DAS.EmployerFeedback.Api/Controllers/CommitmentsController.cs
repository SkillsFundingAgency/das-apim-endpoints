using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerFeedback.Application.Queries.GetProvider;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerFeedback.Api.Controllers
{
    [ApiController]
    [Route("commitments")]
    public class CommitmentsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CommitmentsController> _logger;

        public CommitmentsController(IMediator mediator, ILogger<CommitmentsController> logger)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet("{providerId}")]
        public async Task<IActionResult> GetProvider(int providerId)
        {
            try
            {
                _logger.LogInformation("Getting providers");
                if (providerId <= 0)
                {
                    _logger.LogWarning("No provider found for providerId: {ProviderId}", providerId);
                    return NotFound();
                }

                var providerResponse = await _mediator.Send(new GetProviderQuery
                {
                    ProviderId = providerId
                });

                if (providerResponse == null)
                {
                    _logger.LogWarning("No provider found for providerId: {ProviderId}", providerId);
                    return NotFound();
                }

                return Ok(providerResponse);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to retrieve provider");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }

        }
    }
}
