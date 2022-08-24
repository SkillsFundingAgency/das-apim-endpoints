using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.RoatpCourseManagement.Application.Locations.Commands.CreateProviderLocation;
using SFA.DAS.RoatpCourseManagement.Application.Standards.Commands.AddNationalLocation;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.Api.Controllers
{
    [ApiController]
    public class ProviderCourseLocationCreateController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ProviderCourseLocationCreateController> _logger;

        public ProviderCourseLocationCreateController(ILogger<ProviderCourseLocationCreateController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost]
        [Route("providers/{ukprn}/courses/{larsCode}/add-national-location")]
        public async Task<IActionResult> AddNationalLocationToProviderCourseLocations([FromRoute] int ukprn, [FromRoute] int larsCode, [FromBody] AddNationalLocationToProviderCourseLocationsCommand command)
        {
            _logger.LogInformation("Outer API: Request received to add national location for ukprn: {ukprn} larscode: {larscode} by user: {userId}", ukprn, larsCode, command.UserId);

            command.Ukprn = ukprn;
            command.LarsCode = larsCode;

            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPost]
        [Route("providers/{ukprn}/courses/{larsCode}/create-providercourselocation")]
        public async Task<IActionResult> CreateProviderCourseLocation([FromRoute] int ukprn, [FromRoute] int larsCode, [FromBody] AddProviderCourseLocationCommand command)
        {
            _logger.LogInformation("Outer API: Request received to create provider course location for ukprn: {ukprn} larscode: {larscode} locationNavigationId : {locationNavigationId} by user: {userId}", ukprn, larsCode, command.LocationNavigationId, command.UserId);

            command.Ukprn = ukprn;
            command.LarsCode = larsCode;

            await _mediator.Send(command);
            return new StatusCodeResult(StatusCodes.Status201Created);
        }
    }
}
