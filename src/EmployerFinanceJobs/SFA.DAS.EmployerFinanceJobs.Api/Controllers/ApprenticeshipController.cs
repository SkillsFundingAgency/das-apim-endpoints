using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerFinanceJobs.Api.Models.Requests;
using SFA.DAS.EmployerFinanceJobs.Commands.ImportLearnersByAccountId;

namespace SFA.DAS.EmployerFinanceJobs.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ApprenticeshipController(ILogger<ApprenticeshipController> logger,
    IMediator mediator) : ControllerBase
{
    [HttpPost]
    [Route("import-learners")]
    [ProducesResponseType(typeof(ImportLearnersByAccountIdResult), 200)]
    [ProducesResponseType(typeof(ImportLearnersByAccountIdResult), 400)]
    [ProducesResponseType(typeof(ImportLearnersByAccountIdResult), 500)]
    public async Task<IResult> ImportLearnersByAccountId(
        [FromBody] PostImportLearnersByAccountIdRequest request)
    {
        logger.LogInformation("Initiating import of learners for account ID {AccountId}", request.AccountId);

        var result = await mediator.Send(new ImportLearnersByAccountIdCommand(request.AccountId, request.LastSuccessfulImportDate));

        return TypedResults.Ok(result);
    }
}