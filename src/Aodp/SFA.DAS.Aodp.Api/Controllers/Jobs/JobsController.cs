using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aodp.Application.Queries.Jobs;
using SFA.DAS.Aodp.Application.Queries.Qualifications;
using SFA.DAS.AODP.Domain.Qualifications.Requests;

namespace SFA.DAS.Aodp.Api.Controllers.JobRuns
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : BaseController
    {
        public JobsController(IMediator mediator, ILogger<JobRunsController> logger) : base(mediator, logger) { }
        
        [HttpGet("/api/job")]
        [ProducesResponseType(typeof(GetJobByNameQueryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByNameAsync([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                _logger.LogWarning("Job name is empty");
                return BadRequest(new { message = "Job name cannot be empty" });
            }

            var query = new GetJobByNameQuery() { JobName = name };
            return await SendRequestAsync(query);
        }
    }
}
