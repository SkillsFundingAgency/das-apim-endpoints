using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Reservations.Application.Cohorts.Queries.GetCohortAccess;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Authorization;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Commitments;

namespace SFA.DAS.Reservations.Api.Controllers;

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
}