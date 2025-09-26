using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.DeleteApplication;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.PatchApplication;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.PatchApplicationStatus;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.PatchApplicationDisabilityConfidence;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.PatchApplicationTrainingCourses;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.PatchApplicationVolunteeringAndWorkHistory;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.SubmitApplication;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Applications.GetApplication;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.WithdrawApplication;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetApplication;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetApplicationSubmitted;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.Index;
using SFA.DAS.FindAnApprenticeship.Application.Queries.DeleteApplication;
using SFA.DAS.FindAnApprenticeship.Application.Queries.WithdrawApplication;
using SFA.DAS.FindAnApprenticeship.Domain.Models;

namespace SFA.DAS.FindAnApprenticeship.Api.Controllers;

[ApiController]
[Route("[controller]s/{applicationId}")]
public class ApplicationController(IMediator mediator, ILogger<ApplicationController> logger) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index([FromRoute] Guid applicationId, [FromQuery] Guid candidateId)
    {
        try
        {
            var result = await mediator.Send(new GetIndexQuery
                { CandidateId = candidateId, ApplicationId = applicationId });

            if (result == null) return NotFound();

            return Ok((GetIndexApiResponse)result);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Error getting application index {applicationId}", applicationId);
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpGet("{candidateId}/summary")]
    public async Task<IActionResult> GetApplicationSummary([FromRoute] Guid applicationId, [FromRoute] Guid candidateId)
    {
        try
        {
            var result = await mediator.Send(new GetApplicationQuery
                { CandidateId = candidateId, ApplicationId = applicationId });

            if (result == null) return NotFound();

            return Ok((GetApplicationApiResponse)result);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Error getting GetApplicationDetails {applicationId}", applicationId);
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpGet("{candidateId}/view")]
    public async Task<IActionResult> GetApplication([FromRoute] Guid applicationId, [FromRoute] Guid candidateId)
    {
        try
        {
            var result = await mediator.Send(new GetApplicationViewQuery
                { CandidateId = candidateId, ApplicationId = applicationId });

            if (result == null) return NotFound();

            return Ok((GetApplicationViewApiResponse)result);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting GetApplication {Id}", applicationId);
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpPost("{candidateId}/status")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateApplicationStatus(
        [FromRoute] Guid applicationId,
        [FromRoute] Guid candidateId,
        [FromBody] UpdateApplicationStatusModel request,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new PatchApplicationStatusCommand
        {
            ApplicationId = applicationId,
            CandidateId = candidateId,
            Status = request.Status
        }, cancellationToken);

        if (result.Application == null)
        {
            return NotFound();
        }

        return Ok(result.Application);
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
        var result = await mediator.Send(new PatchApplicationWorkHistoryCommand
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
        var result = await mediator.Send(new PatchApplicationTrainingCoursesCommand
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
        var result = await mediator.Send(new PatchApplicationVolunteeringAndWorkExperienceCommand
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
        var result = await mediator.Send(new PatchApplicationDisabilityConfidenceCommand
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

    [HttpPost("preview")]
    public async Task<IActionResult> SubmitApplication(
        [FromRoute] Guid applicationId,
        [FromQuery] Guid candidateId)
    {
        try
        {
            await mediator.Send(new SubmitApplicationCommand
            {
                ApplicationId = applicationId,
                CandidateId = candidateId
            });
            
            return Created();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Error submitting submitted {applicationId}", applicationId);
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
            
    }

    [HttpGet("submitted")]
    public async Task<IActionResult> ApplicationSubmitted([FromRoute] Guid applicationId, [FromQuery] Guid candidateId)
    {
        try
        {
            var result = await mediator.Send(new GetApplicationSubmittedQuery
            {
                ApplicationId = applicationId,
                CandidateId = candidateId
            });

            if (result == null) return NotFound();

            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Error getting application submitted {applicationId}", applicationId);
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpGet("withdraw")]
    public async Task<IActionResult> WithdrawApplication([FromRoute] Guid applicationId, [FromQuery] Guid candidateId)
    {
        try
        {
            var result = await mediator.Send(new WithdrawApplicationQuery
                { ApplicationId = applicationId, CandidateId = candidateId });

            if (result == null || result.ApplicationId == Guid.Empty)
            {
                return NotFound();
            }
                
            return Ok((GetWithdrawnApplicationApiResponse)result);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Error getting withdrawn application {applicationId}", applicationId);
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }
        
    [HttpPost("withdraw")]
    public async Task<IActionResult> WithdrawApplicationPost([FromRoute] Guid applicationId, [FromQuery] Guid candidateId)
    {
        try
        {
            await mediator.Send(new WithdrawApplicationCommand
            {
                ApplicationId = applicationId,
                CandidateId = candidateId
            });
            return Ok();
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Error posting withdrawn application {applicationId}", applicationId);
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }
        
    [HttpGet("delete")]
    public async Task<IActionResult> ConfirmDeleteApplication([FromRoute] Guid applicationId, [FromQuery] Guid candidateId, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new ConfirmDeleteApplicationQuery(candidateId, applicationId), cancellationToken);
        return result == ConfirmDeleteApplicationQueryResult.None
            ? NotFound()
            : Ok(new GetConfirmDeleteApplicationApiResponse(result));
    }
        
    [HttpPost("delete")]
    public async Task<IActionResult> DeleteApplication([FromRoute] Guid applicationId, [FromQuery] Guid candidateId, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DeleteApplicationCommand(candidateId, applicationId), cancellationToken);
        return result == DeleteApplicationCommandResult.None
            ? NotFound()
            : Ok(new PostDeleteApplicationApiResponse(result.EmployerName, result.VacancyTitle));
    }
}