using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.CreateWorkExperience;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetWorkExperiences;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Api.Controllers
{
    [ApiController]
    [Route("applications/{applicationId}/[controller]")]
    public class WorkExperiencesController(IMediator mediator, ILogger<WorkExperiencesController> logger)
        : Controller
    {
        [HttpGet]
        public async Task<IActionResult> GetWorkExperiences([FromRoute] Guid applicationId, [FromQuery] Guid candidateId)
        {
            try
            {
                var result = await mediator.Send(new GetVolunteeringAndWorkExperiencesQuery
                {
                    CandidateId = candidateId,
                    ApplicationId = applicationId,
                });

                if (result is null) return NotFound();
                return Ok(result);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error getting work experiences : An error occurred");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostWorkExperience([FromRoute] Guid applicationId, [FromBody] PostWorkExperienceApiRequest request)
        {
            try
            {
                var result = await mediator.Send(new CreateWorkCommand
                {
                    ApplicationId = applicationId,
                    CandidateId = request.CandidateId,
                    Description = request.Description,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    CompanyName = request.CompanyName
                });

                if (result == null)
                {
                    return NotFound();
                }

                return Created(result.Id.ToString(), (PostWorkExperienceApiResponse)result);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error posting work experience for application {applicationId}", applicationId);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
