using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aodp.Application.Queries.Jobs;
using SFA.DAS.Aodp.Application.Queries.Qualifications;

namespace SFA.DAS.Aodp.Api.Controllers.JobRuns
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobRunsContoller : BaseController
    {
        public JobRunsContoller(IMediator mediator, ILogger<JobRunsContoller> logger) : base(mediator, logger) { }

        [HttpGet("/api/job-runs/{jobName}")]
        [ProducesResponseType(typeof(GetJobRunsByNameQueryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetJobRuns(string jobName)
        {
            if (string.IsNullOrWhiteSpace(jobName))
            {
                return BadRequest(new { message = "Job name cannot be empty" });
                _logger.LogWarning("Job name is empty");
            }

            var result = await _mediator.Send(new GetJobRunsByNameQuery { JobName = jobName });

            if (!result.Success || result.Value == null)
            {
                _logger.LogWarning(result.ErrorMessage);
                return NotFound(new { message = $"No job runs found for job name: {jobName}" });
            }

            return Ok(result.Value);
        }
    }
}
