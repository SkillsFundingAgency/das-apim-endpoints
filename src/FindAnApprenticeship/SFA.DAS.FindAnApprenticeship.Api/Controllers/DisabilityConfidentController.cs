using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using System.Net;
using System.Threading.Tasks;
using System;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.DisabilityConfident;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.DisabilityConfident;

namespace SFA.DAS.FindAnApprenticeship.Api.Controllers
{
    [ApiController]
    [Route("applications/{applicationId}/disability-confident")]
    public class DisabilityConfidentController(IMediator mediator, ILogger<DisabilityConfidentController> logger)
        : Controller
    {

        [HttpGet]
        public async Task<IActionResult> GetDisabilityConfident([FromRoute] Guid applicationId, [FromQuery] Guid candidateId)
        {
            try
            {
                var result = await mediator.Send(new GetDisabilityConfidentQuery
                {
                    CandidateId = candidateId,
                    ApplicationId = applicationId,
                });

                if (result is null) return NotFound();
                return Ok((GetDisabilityConfidentApiResponse)result);
            }
            catch (Exception e)
            {
                logger.LogError(e, "GetDisabilityConfident : An error occurred");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("details")]
        public async Task<IActionResult> GetDisabilityConfidentDetails([FromRoute] Guid applicationId, [FromQuery] Guid candidateId)
        {
            try
            {
                var result = await mediator.Send(new GetDisabilityConfidentDetailsQuery
                {
                    CandidateId = candidateId,
                    ApplicationId = applicationId,
                });

                if (result is null) return NotFound();
                return Ok((GetDisabilityConfidentDetailsApiResponse)result);
            }
            catch (Exception e)
            {
                logger.LogError(e, "GetDisabilityConfidentDetails : An error occurred");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost("")]
        public async Task<IActionResult> PostDisabilityConfident([FromRoute] Guid applicationId, [FromBody] PostDisabilityConfidentApiRequest request)
        {
            try
            {
                await mediator.Send(new UpdateDisabilityConfidentCommand
                {
                    ApplicationId = applicationId,
                    CandidateId = request.CandidateId,
                    ApplyUnderDisabilityConfidentScheme = request.ApplyUnderDisabilityConfidentScheme
                });

                return Ok();
            }
            catch (Exception e)
            {
                logger.LogError(e, "PostDisabilityConfident : An error occurred");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
