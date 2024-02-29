using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.RoatpCourseManagement.Application.Locations.Commands.UpdateProviderLocationDetails;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.Api.Controllers
{
    [ApiController]
    public class ProviderLocationsEditController : ControllerBase
    {
        private readonly ILogger<ProviderLocationsEditController> _logger;
        private readonly IMediator _mediator;

        public ProviderLocationsEditController(ILogger<ProviderLocationsEditController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost]
        [Route("providers/{ukprn}/locations/{id}/update-provider-location-details")]
        public async Task<IActionResult> UpdateProviderLocationDetails([FromRoute] int ukprn, [FromRoute] Guid id, UpdateProviderLocationDetailsCommand command)
        {
            _logger.LogInformation("Outer API: Request to update provider location details for ukprn: {ukprn} id: {id}", ukprn, id);
            command.Ukprn = ukprn;
            command.Id = id;
            var httpStatusCode = await _mediator.Send(command);
            if (httpStatusCode != HttpStatusCode.NoContent)
            {
                _logger.LogError("Outer API: Failed request to update provider location details for ukprn: {ukprn} id: {id} with HttpStatusCode: {httpstatuscode}", ukprn, id, httpStatusCode);
                return new StatusCodeResult((int)httpStatusCode);
            }
            return NoContent();
        }
    }
}
