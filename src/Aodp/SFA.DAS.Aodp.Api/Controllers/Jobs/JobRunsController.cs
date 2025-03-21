using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aodp.Application.Queries.Jobs;
using SFA.DAS.Aodp.Application.Queries.Qualifications;

namespace SFA.DAS.Aodp.Api.Controllers.JobRuns
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobRunsController : BaseController
    {
        public JobRunsController(IMediator mediator, ILogger<JobRunsController> logger) : base(mediator, logger) { }

        [HttpGet("/api/job/{jobName}/runs")]
        [ProducesResponseType(typeof(GetJobRunsQueryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetJobRuns(string jobName)
        {
            if (string.IsNullOrWhiteSpace(jobName))
            {
                _logger.LogWarning("Job name is empty");
                return BadRequest(new { message = "Job name cannot be empty" });                
            }

            return await SendRequestAsync(new GetJobRunsQuery { JobName = jobName });
        }

        [HttpPost("/api/job/requestrun")]
        [ProducesResponseType(typeof(EmptyResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RequestJobRuns([FromBody] RequestJobRunCommand command)
        {
            if (string.IsNullOrWhiteSpace(command?.JobName))
            {
                _logger.LogWarning("Job name is empty");
                return BadRequest(new { message = "Job name cannot be empty" });
            }

            return await SendRequestAsync(command);
        }
    }
}
