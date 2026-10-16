using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerFinanceJobs.Api.Models.Requests;
using SFA.DAS.EmployerFinanceJobs.Commands.PostJob;

namespace SFA.DAS.EmployerFinanceJobs.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class JobsController(IMediator mediator) : ControllerBase
{
    [HttpPost("{id:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IResult> PostJob(
        long id,
        [FromBody] PostJobRequest request)
    {
        await mediator.Send(new PostJobCommand(
            id,
            new PostJobCommandData(
                request.AccountId,
                request.JobName,
                request.LastSuccessfulImportDate,
                request.LastAttemptedDate,
                request.LastAttemptSuccessful,
                request.TotalRecordsLastRun,
                request.FailedRecordsLastRun)));

        return TypedResults.Ok();
    }
}