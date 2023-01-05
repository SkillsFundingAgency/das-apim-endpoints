using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Net;
using SFA.DAS.Roatp.CourseManagement.Application.Standards.Commands.UpdateStandardSubRegions;

namespace SFA.DAS.RoatpCourseManagement.Api.Controllers
{
    [ApiController]
    public class ProviderCourseLocationRegionsEditController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ProviderCourseLocationRegionsEditController> _logger;

        public ProviderCourseLocationRegionsEditController(IMediator mediator, ILogger<ProviderCourseLocationRegionsEditController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        [HttpPost]
        [Route("providers/{ukprn}/courses/{larsCode}/locations/regions")]
        public async Task<IActionResult> UpdateStandardSubRegions(int ukprn, int larsCode, UpdateStandardSubRegionsCommand command)
        {
            _logger.LogInformation("Outer API: Request to update standard subregions for ukprn: {ukprn} larscode: {larscode}", ukprn, larsCode);
            command.Ukprn = ukprn;
            command.LarsCode = larsCode;
            var httpStatusCode = await _mediator.Send(command);
            if (httpStatusCode != HttpStatusCode.NoContent)
            {
                _logger.LogError("Outer API: Failed request to update standard subregions for ukprn: {ukprn} larscode: {larscode} with HttpStatusCode: {httpstatuscode}", ukprn, larsCode, httpStatusCode);
                return new StatusCodeResult((int)httpStatusCode);
            }
            return NoContent();
        }
    }
}
