using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerFinanceJobs.Commands.ImportCommittedLearners;
using SFA.DAS.EmployerFinanceJobs.Commands.UpdateLearnerCost;

namespace SFA.DAS.EmployerFinanceJobs.Api.Controllers;

[ApiController]
[Route("learners")]
public class LearnersController(IMediator mediator) : ControllerBase
{
    [HttpPost("import")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ImportCommittedLearners([FromBody] DateTime cutOffDateTime)
    {
        var command = new ImportCommittedLearnersCommand(cutOffDateTime);
        var result = await mediator.Send(command);
        return Ok(result);
    }


    [HttpPost("update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateLearnerCosts()
    {
        var command = new UpdateLearnerCostCommand();
        var result = await mediator.Send(command);
        return Ok(result);
    }
}