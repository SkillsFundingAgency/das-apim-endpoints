using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.UpdateEmploymentLocations;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetEmploymentLocations;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Api.Controllers
{
    [ApiController]
    [Route("applications/{applicationId}/[controller]")]
    public class EmploymentLocationsController(IMediator mediator, ILogger<EmploymentLocationsController> logger) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromRoute] Guid applicationId, [FromQuery] Guid candidateId)
        {
            try
            {
                var result = await mediator.Send(new GetEmploymentLocationsQuery(applicationId, candidateId));

                if (result is null) return NotFound();

                return Ok(result);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Get Employment Locations : An error occurred");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromRoute] Guid applicationId, [FromBody] PostEmploymentLocationsApiRequest request)
        {
            try
            {
                var result = await mediator.Send(new UpdateEmploymentLocationsCommand
                {
                    ApplicationId = applicationId,
                    CandidateId = request.CandidateId,
                    EmploymentLocationSectionStatus = request.EmploymentLocationSectionStatus,
                    EmployerLocation = request.EmployerLocation
                });

                if (result is null) return NotFound();

                return Created($"{result.Id}", result.Application);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error posting employment locations for application:{applicationId}", applicationId);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}