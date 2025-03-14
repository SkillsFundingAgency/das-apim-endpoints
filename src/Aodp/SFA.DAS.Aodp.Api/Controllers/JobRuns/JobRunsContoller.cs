using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aodp.Application.Queries.Jobs;

namespace SFA.DAS.Aodp.Api.Controllers.JobRuns
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobRunsContoller : BaseController
    {
        public JobRunsContoller(IMediator mediator, ILogger<JobRunsContoller> logger) : base(mediator, logger) { }

        [HttpGet("/api/job-runs")]
        [ProducesResponseType(typeof(GetJobRunsQueryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetJobRuns()
        {
            return await SendRequestAsync(new GetJobRunsQuery());
        }
    }
}
