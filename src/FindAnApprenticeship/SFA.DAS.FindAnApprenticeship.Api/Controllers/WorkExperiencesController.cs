using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.CreateWorkExperience;
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
        [HttpPost]
        public async Task<IActionResult> PostWorkExperience([FromRoute] Guid applicationId, [FromBody] PostWorkExperienceApiRequest request)
        {
            try
            {
                var result = await mediator.Send(new CreateWorkCommand
                {
                    ApplicationId = applicationId,
                    CandidateId = request.CandidateId,
                    JobTitle = request.JobTitle,
                    JobDescription = request.JobDescription,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    EmployerName = request.EmployerName
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
