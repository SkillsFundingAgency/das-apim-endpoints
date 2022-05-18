using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.Roatp.CourseManagement.Application.RegisteredProviders.Queries;

namespace SFA.DAS.Roatp.CourseManagement.Api.Controllers
{

        [ApiController]
        public class RegisteredProviderController : ControllerBase
        {
            private readonly ILogger<RegisteredProviderController> _logger;
            private readonly IMediator _mediator;

            public RegisteredProviderController(ILogger<RegisteredProviderController> logger, IMediator mediator)
            {
                _logger = logger;
                _mediator = mediator;
            }

            [HttpGet]
            [Route("providers")]
            public async Task<IActionResult> GetRegisteredProviders()
            {
                _logger.LogInformation("Request received for all locations from roatp");
                var query = new GetRegisteredProvidersQuery();
                var result = await _mediator.Send(query);
                if (result.Providers == null)
                {
                    _logger.LogInformation("No providers returned from roatp");
                    return BadRequest();
                }
                _logger.LogInformation($"Found {result.Providers.Count} providers");
                return Ok(result.Providers);
            }
        }
}
