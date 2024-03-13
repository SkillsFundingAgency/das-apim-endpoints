using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.Application.Authorization.Queries;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Authorization;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Commitments;

namespace SFA.DAS.Approvals.Api.Controllers;

[ApiController]
[Route("[controller]/")]
public class AuthorizationController(ISender mediator, ILogger<AuthorizationController> logger) : Controller
{
    [HttpGet]
    [Route("{partyId}/can-access-cohort/{cohortId}")]
    public async Task<IActionResult> CanAccessCohort(long partyId, long cohortId, [FromQuery] Party party)
    {
        try
        {
            var result = await mediator.Send(new GetCohortAccessQuery(party, partyId, cohortId));
            return Ok(new GetCohortAccessResponse { HasCohortAccess = result });
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "{method} threw an exception.", nameof(CanAccessCohort));
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpGet]
    [Route("{partyId}/can-access-apprenticeship/{apprenticeshipId}")]
    public async Task<IActionResult> CanAccessApprenticeship(long partyId, long apprenticeshipId, [FromQuery] Party party)
    {
        try
        {
            var result = await mediator.Send(new GetApprenticeshipAccessQuery(party, partyId, apprenticeshipId));
            return Ok(new GetApprenticeshipAccessResponse { HasApprenticeshipAccess = result });
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "{method} threw an exception.", nameof(CanAccessApprenticeship));
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }
}