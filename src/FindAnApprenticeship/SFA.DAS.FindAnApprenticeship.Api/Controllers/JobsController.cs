using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.CreateJob;

namespace SFA.DAS.FindAnApprenticeship.Api.Controllers
{
    [ApiController]
    [Route("applications/{applicationId}/[controller]")]
    public class JobsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<JobsController> _logger;

        public JobsController(IMediator mediator, ILogger<JobsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> PostJob([FromRoute] Guid applicationId, [FromBody] PostJobApiRequest request)
        {
            try
            {
                var result = await _mediator.Send(new CreateJobCommand
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

                return Created(result.Id.ToString(), (PostJobApiResponse)result);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error posting job for application {applicationId}", applicationId);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
