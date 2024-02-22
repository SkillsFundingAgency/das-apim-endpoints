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
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.PatchApplicationSkillsAndStrengths;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.PatchApplicationTrainingCourses;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.PatchApplicationVolunteeringAndWorkHistory;
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

        [HttpPost("{candidateId}/skills-and-strengths")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateSkillsAndStrengths(
            [FromRoute] Guid applicationId,
            [FromRoute] Guid candidateId,
            [FromBody] UpdateSkillsAndStrengthsApplicationModel request,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new PatchApplicationSkillsAndStrengthsCommand
            {
                ApplicationId = applicationId,
                CandidateId = candidateId,
                SkillsAndStrengthsSectionStatus = request.SkillsAndStrengthsSctionStatus
            }, cancellationToken);

            if (result.Application == null)
            {
                return NotFound();
            }

            return Ok(result.Application);
        }
    }
}
