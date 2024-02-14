using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.CreateJob;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.UpdateJob;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetJob;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.WorkHistory;

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

        [HttpGet]
        public async Task<IActionResult> GetJobs([FromRoute] Guid applicationId, [FromQuery] Guid candidateId)
        {
            try
            {
                var result = await _mediator.Send(new GetJobsQuery
                {
                    CandidateId = candidateId,
                    ApplicationId = applicationId,
                });
                return Ok((GetJobsApiResponse)result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Get Job Histories : An error occurred");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
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

        [HttpGet]
        [Route("{jobId}")]
        public async Task<IActionResult> GetJob([FromRoute] Guid applicationId, [FromRoute] Guid jobId, [FromQuery] Guid candidateId)
        {
            try
            {
                var result = await _mediator.Send(new GetJobQuery
                {
                    CandidateId = candidateId,
                    ApplicationId = applicationId,
                    JobId = jobId
                });
                return Ok((GetJobApiResponse)result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Get Job : An error occurred");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("{jobId}")]
        public async Task<IActionResult> PostUpdateJob([FromRoute] Guid applicationId, [FromRoute] Guid jobId, [FromBody] PostUpdateJobApiRequest request)
        {
            try
            {
                await _mediator.Send(new UpdateJobCommand
                {
                    ApplicationId = applicationId,
                    JobId = jobId,
                    CandidateId = request.CandidateId,
                    Employer = request.EmployerName,
                    Description = request.JobDescription,
                    JobTitle = request.JobTitle,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate
                });
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Update Job : An error occurred");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
