using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetProviderCourseLocation;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.Api.Controllers
{
    [ApiController]
    public class ProviderCourseLocationsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ProviderCourseLocationsController> _logger;

        public ProviderCourseLocationsController(ILogger<ProviderCourseLocationsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("providers/{ukprn}/courses/{larsCode}/provider-locations")]
        public async Task<IActionResult> GetProviderCourseLocations(int ukprn, int larsCode)
        {
            _logger.LogInformation("Outer API: Request received to get provider course locations for ukprn: {ukprn} larscode: {larscode}", ukprn, larsCode);
            var providerCourselocationsResult = await _mediator.Send(new GetProviderCourseLocationQuery(ukprn, larsCode));
            if (providerCourselocationsResult == null || !providerCourselocationsResult.ProviderCourseLocations.Any())
            {
                _logger.LogError($"Provider Course Locations not found for ukprn {ukprn} and lars code {larsCode}");
                return NotFound();
            }

            return Ok(providerCourselocationsResult);
        }
    }
}
