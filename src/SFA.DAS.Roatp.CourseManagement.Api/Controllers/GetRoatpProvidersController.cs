using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.Roatp.CourseManagement.Application.Providers.Queries;

namespace SFA.DAS.Roatp.CourseManagement.Api.Controllers
{

        [ApiController]
        public class GetRoatpProvidersController : ControllerBase
        {
            private readonly ILogger<GetRoatpProvidersController> _logger;
            private readonly IMediator _mediator;

            public GetRoatpProvidersController(ILogger<GetRoatpProvidersController> logger, IMediator mediator)
            {
                _logger = logger;
                _mediator = mediator;
            }

            [HttpGet]
            [Route("providers")]
            public async Task<IActionResult> GetAllProviders()
            {
                _logger.LogInformation("Request received for all locations from roatp");
                var query = new GetAllRoatpProvidersQuery();
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
