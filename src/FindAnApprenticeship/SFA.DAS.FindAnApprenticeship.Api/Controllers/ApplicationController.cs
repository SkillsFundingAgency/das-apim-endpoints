using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.PatchApplication;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.Index;

namespace SFA.DAS.FindAnApprenticeship.Api.Controllers
{
    [ApiController]
    [Route("[controller]s/{applicationId}")]
    public class ApplicationController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ApplicationController> _logger;

        public ApplicationController(IMediator mediator, ILogger<ApplicationController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromRoute] Guid applicationId, [FromQuery] Guid candidateId)
        {
            try
            {
                var result = await _mediator.Send(new GetIndexQuery
                    { CandidateId = candidateId, ApplicationId = applicationId });

                return Ok((GetIndexApiResponse)result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error getting application index {applicationId}", applicationId);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
  
        }

        [HttpPost("{candidateId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateApplication(
            [FromRoute] Guid applicationId,
            [FromRoute] Guid candidateId,
            [FromBody] UpdateApplicationModel request,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new PatchApplicationCommand
            {
                ApplicationId = applicationId,
                CandidateId = candidateId,
                WorkExperienceStatus = request.WorkHistorySectionStatus
            }, cancellationToken);

            if (result.Application == null)
            {
                return NotFound();
            }

            return Ok(result.Application);
        }
    }
}
