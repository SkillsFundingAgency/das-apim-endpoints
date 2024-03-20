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
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.PatchApplicationDisabilityConfidence;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.PatchApplicationTrainingCourses;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.PatchApplicationVolunteeringAndWorkHistory;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetApplicationSubmitted;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.Index;
using SFA.DAS.FindAnApprenticeship.Models;

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

        [HttpPost("{candidateId}/work-history")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateApplicationWorkHistory(
            [FromRoute] Guid applicationId,
            [FromRoute] Guid candidateId,
            [FromBody] UpdateApplicationWorkHistoryModel request,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new PatchApplicationWorkHistoryCommand
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

        [HttpPost("{candidateId}/training-courses")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateApplicationTrainingCourses(
            [FromRoute] Guid applicationId,
            [FromRoute] Guid candidateId,
            [FromBody] UpdateApplicationTrainingCoursesModel request,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new PatchApplicationTrainingCoursesCommand
            {
                ApplicationId = applicationId,
                CandidateId = candidateId,
                TrainingCoursesStatus = request.TrainingCoursesSectionStatus
            }, cancellationToken);

            if (result.Application == null)
            {
                return NotFound();
            }

            return Ok(result.Application);
        }

        [HttpPost("{candidateId}/volunteering-and-work-experience")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateVolunteeringAndWorkExperience(
            [FromRoute] Guid applicationId,
            [FromRoute] Guid candidateId,
            [FromBody] UpdateVolunteeringAndWorkExperienceModel request,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new PatchApplicationVolunteeringAndWorkExperienceCommand
            {
                ApplicationId = applicationId,
                CandidateId = candidateId,
                VolunteeringAndWorkExperienceStatus = request.VolunteeringAndWorkExperienceSectionStatus
            }, cancellationToken);

            if (result.Application == null)
            {
                return NotFound();
            }

            return Ok(result.Application);
        }

        [HttpPost("{candidateId}/disability-confidence")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateDisabilityConfidence(
            [FromRoute] Guid applicationId,
            [FromRoute] Guid candidateId,
            [FromBody] UpdateDisabilityConfidenceModel request,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new PatchApplicationDisabilityConfidenceCommand
            {
                ApplicationId = applicationId,
                CandidateId = candidateId,
                DisabilityConfidenceStatus = request.IsSectionCompleted 
                    ? SectionStatus.Completed 
                    : SectionStatus.Incomplete
            }, cancellationToken);

            if (result.Application == null)
            {
                return NotFound();
            }

            return Ok(result.Application);
        }

        [HttpGet("submitted")]
        public async Task<IActionResult> ApplicationSubmitted([FromRoute] Guid applicationId, [FromQuery] Guid candidateId)
        {
            try
            {
                var result = await _mediator.Send(new GetApplicationSubmittedQuery
                {
                    ApplicationId = applicationId,
                    CandidateId = candidateId
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting application submitted {applicationId}", applicationId);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
