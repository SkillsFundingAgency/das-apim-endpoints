using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.UpdateQualifications;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.Qualifications;

namespace SFA.DAS.FindAnApprenticeship.Api.Controllers
{

    [ApiController]
    [Route("applications/{applicationId}/qualifications")]
    public class QualificationsController(IMediator mediator, ILogger<QualificationsController> logger)
        : Controller
    {
        [HttpGet("")]
        public async Task<IActionResult> GetQualifications([FromRoute] Guid applicationId, [FromQuery] Guid candidateId)
        {
            try
            {
                var result = await mediator.Send(new GetQualificationsQuery
                {
                    ApplicationId = applicationId,
                    CandidateId = candidateId
                });

                return Ok((GetQualificationsApiResponse)result);
            }
            catch (Exception e)
            {
                logger.LogError(e, "GetQualifications : An error occurred");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost("")]
        public async Task<IActionResult> PostQualifications([FromRoute] Guid applicationId, [FromBody] PostQualificationsApiRequest request)
        {
            try
            {
                await mediator.Send(new UpdateQualificationsCommand
                {
                    ApplicationId = applicationId,
                    CandidateId = request.CandidateId,
                    IsComplete = request.IsComplete
                });

                return Created();
            }
            catch (Exception e)
            {
                logger.LogError(e, "PostQualifications : An error occurred");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

    }
}
