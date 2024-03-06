using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Approvals.Application.Authorization.Queries;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Authorization;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Authorization;

namespace SFA.DAS.Approvals.Api.Controllers;

[ApiController]
[Route("[controller]/")]
public class AuthorizationController(ISender mediator) : Controller
{
    [HttpGet]
    [Route(nameof(CanAccessCohort))]
    public async Task<IActionResult> CanAccessCohort([FromBody] GetCohortAccessRequest request)
    {
        try
        {
            var result = await mediator.Send(new GetCohortAccessQuery(request.Party, request.PartyId, request.CohortId));
            return Ok(new GetCohortAccessResponse { HasCohortAccess = result });
        }
        catch (Exception)
        {
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpGet]
    [Route(nameof(CanAccessApprenticeship))]
    public async Task<IActionResult> CanAccessApprenticeship([FromBody] GetApprenticeshipAccessRequest request)
    {
        try
        {
            var result = await mediator.Send(new GetApprenticeshipAccessQuery(request.Party, request.PartyId, request.ApprenticeshipId));
            return Ok(new GetApprenticeshipAccessResponse { HasApprenticeshipAccess = result });
        }
        catch (Exception)
        {
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }
}