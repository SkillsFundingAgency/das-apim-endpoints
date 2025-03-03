using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Approvals.Application.ProviderPermissions.Queries;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ProviderRelationships;

namespace SFA.DAS.Approvals.Api.Controllers;

[ApiController]
[Route("[controller]/")]
public class ProviderPermissionsController(ISender mediator) : Controller﻿Base
{
    [HttpGet]
    [Route("has-permission")]
    public async Task<IActionResult> HasPermission([FromQuery] long ukPrn, [FromQuery] long accountLegalEntityId, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetHasPermissionQuery(ukPrn, accountLegalEntityId), cancellationToken);
        return Ok(new GetHasPermissionResponse { HasPermission = result });
    }

    [HttpGet]
    [Route("has-relationship-with-permission")]
    public async Task<IActionResult> HasRelationshipWithPermission([FromQuery] long ukprn, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetHasRelationshipWithPermissionQuery(ukprn), cancellationToken);
        return Ok(new GetHasRelationshipWithPermissionResponse { HasPermission = result });
    }

    [HttpGet]
    [Route("account-provider-legal-entities")]
    public async Task<IActionResult> AccountProviderLegalEntities([FromQuery] int ukprn, CancellationToken cancellationToken)
    {
        var result =
            await mediator.Send(new GetAccountProviderLegalEntitiesQuery(ukprn), cancellationToken);
        return Ok(result);
    }
}
