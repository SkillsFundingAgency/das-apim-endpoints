using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NLog;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.CreateJob;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.DeleteJob;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.UpdateJob;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetJob;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.WorkHistory;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.WorkHistory.DeleteJob;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;

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

                if (result is null) return NotFound();
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

                if (result is null) return NotFound();

                return Created($"{result.Id}", (PostJobApiResponse)result);
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

                if (result is null) return NotFound();
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
                var result = await _mediator.Send(new UpdateJobCommand
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

                if (result is null) return NotFound();

                return Ok(result.Id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Update Job : An error occurred");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("{jobId}/delete")]
        public async Task<IActionResult> GetDeleteJob([FromRoute] Guid applicationId, [FromQuery] Guid candidateId, [FromRoute] Guid jobId)
        {
            try
            {
                var result = await _mediator.Send(new GetDeleteJobQuery
                {
                    CandidateId = candidateId,
                    ApplicationId = applicationId,
                    JobId = jobId
                });
                return Ok((Models.Applications.GetDeleteJobApiResponse)result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Get Job : An error occurred");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }



        [HttpPost("{jobId}/delete")]
        public async Task<IActionResult> PostDeleteJob([FromRoute] Guid applicationId, [FromRoute]Guid jobId, [FromBody]PostDeleteJobRequest request)
        {
            try
            {
                var result = await _mediator.Send(new PostDeleteJobCommand
                {
                    ApplicationId = applicationId,
                    JobId = jobId,
                    CandidateId = request.CandidateId
                });

                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "DeleteJob : An error occurred");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
