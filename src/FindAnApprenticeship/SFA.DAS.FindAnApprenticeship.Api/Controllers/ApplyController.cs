using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.Index;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.PatchApplication;

namespace SFA.DAS.FindAnApprenticeship.Api.Controllers
{
    [ApiController]
    [Route("vacancies/{vacancyReference}/[controller]")]
    public class ApplyController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ApplyController> _logger;

        public ApplyController(IMediator mediator, ILogger<ApplyController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string vacancyReference, [FromQuery] string applicantEmailAddress)
        {
            var result = await _mediator.Send(new GetIndexQuery
                { ApplicantEmailAddress = applicantEmailAddress, VacancyReference = vacancyReference });

            return Ok((GetIndexApiResponse) result);
        }

        [HttpPost("{applicationId}/{candidateId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateApplication(
            [FromRoute] string vacancyReference,
            [FromRoute] Guid applicationId,
            [FromRoute] Guid candidateId,
            [FromBody] UpdateApplicationModel request,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new PatchApplicationCommand
            {
                ApplicationId = applicationId,
                CandidateId = candidateId,
                VacancyReference = vacancyReference,
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
