using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.RoatpCourseManagement.Application.Locations.Commands.CreateProviderLocation;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.Api.Controllers
{
    [ApiController]
    public class ProviderLocationCreateController : ControllerBase
    {
        public readonly IMediator _mediator;
        public readonly ILogger<ProviderCourseLocationCreateController> _logger;

        public ProviderLocationCreateController(IMediator mediator, ILogger<ProviderCourseLocationCreateController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        [Route("providers/{ukprn}/locations/create-provider-location")]
        public async Task<IActionResult> CreateProviderLocation([FromRoute] int ukprn, [FromBody] CreateProviderLocationCommand command)
        {
            _logger.LogInformation("Outer API: Request received to create provider location {locationName} for ukprn: {ukprn} by user: {userId}", command.LocationName, ukprn, command.UserId);

            command.Ukprn = ukprn;

            await _mediator.Send(command);
            return new StatusCodeResult(StatusCodes.Status201Created);
        }
    }
}
