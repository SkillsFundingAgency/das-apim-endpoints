using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.Api.Models;
using SFA.DAS.Approvals.Application.SelectMultiple.Queries;

namespace SFA.DAS.Approvals.Api.Controllers;

[ApiController]
[Route("[controller]/")]
public class SelectMultipleController(ILogger<SelectMultipleController> logger, IMediator mediator) : Controller
{

    [HttpPost]
    [Route("Validate")]
    public async Task<IActionResult> Validate(SelectMultipleValidateApimRequest request)
    {
        var result = await mediator.Send(
            new ValidateSelectMultipleLearnerRecordsQuery
            {
                ProviderId = request.ProviderId,
                AccountLegalEntityId = request.AccountLegalEntityId,
                LearnerIds = request.LearnerIds,

            });

        return Ok(result);
    }
}